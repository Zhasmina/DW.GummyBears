﻿using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GummyBears.Entities;
using GummyBears.Contracts;
using GummyBears.WebApi.Helpers;
using System.Transactions;

namespace GummyBears.WebApi.Controllers
{
    [RoutePrefix("groups")]
    public class GroupsController : BaseController
    {
        public GroupsController(IDbContext dbContext)
            : base(dbContext)
        { }

        [HttpGet]
        [Route("{groupId:int}")]
        [AuthenticationTokenFilter]
        public async Task<IEnumerable<GroupMessage>> GetMessagesInGroup(int groupId, [FromUri]int userId)
        {
            GroupUserEntity userGroup = await DbContext.GroupsUsersRepo.GetByUserIdAndGroupId(userId, groupId);

            if (userGroup == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Access is denied.");
            }

            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId);

            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("Group with id {0} was not found.", groupId));
            }

            IEnumerable<GroupMessageEntity> dbMessages = await DbContext.GroupMessagesRepo.GetGroupMessages(groupId);
            var authorsIds = dbMessages.Select(i => i.UserId).Distinct();
            var authors = await DbContext.UsersRepo.GetByKeysAsync(authorsIds);
            var authorIdToNameMapping = authors.ToDictionary(u => u.Id, u => string.Format("{0} {1}", u.FirstName, u.LastName));
            var authorIdToUsernameMapping = authors.ToDictionary(u => u.Id, u => u.UserName);

            return dbMessages.Select(m =>
            {
                var contract = m.ToContract();
                contract.AuthorName = authorIdToNameMapping[m.UserId];
                contract.Username = authorIdToUsernameMapping[m.UserId];
                contract.AuthorId = m.UserId;
                contract.UserId = m.UserId;

                return contract;
            }).ToList();

        }

        [HttpPost]
        [Route("")]
        [AuthenticationTokenFilter]
        public async Task<Group> CreateGroup([FromBody]Group group)
        {
            ValidateUserAsAuthenticated(group.AuthorId);

            var createdGroup = await DbContext.GroupsRepo.CreateAsync(group.ToEntity());
            await DbContext.GroupsUsersRepo.CreateAsync(new GroupUserEntity
            {
                GroupId = createdGroup.Id,
                UserId = group.AuthorId,
                IsAdmin = true
            });

            return group;
        }

        [HttpPost]
        [Route("{groupId:int}/participants")]
        [AuthenticationTokenFilter]
        public async Task<GroupParticipants> AddParticipants(int groupId, [FromBody]int participantId)
        {
            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId);

            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.BadRequest, string.Format("Group with id {0} not exists.", groupId));
            }

            ValidateUserAsAuthenticated(group.AuthorId);

            GroupUserEntity participant = await DbContext.GroupsUsersRepo.CreateAsync(new GroupUserEntity
            {
                GroupId = groupId,
                UserId = participantId,
                IsAdmin = false
            });

            return participant.ToContract();
        }

        [HttpGet]
        [Route("{groupId:int}/participants")]
        [AuthenticationTokenFilter]
        public async Task<IEnumerable<GroupParticipants>> GetParticipants(int groupId)
        {
            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId);
            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.BadRequest, string.Format("Group with id {0} not exists.", groupId));
            }

            IEnumerable<GroupUserEntity> groupUsers = await DbContext.GroupsUsersRepo.GetByGroupId(groupId);

            var authorsIds = groupUsers.Select(i => i.UserId).Distinct();
            var authors = await DbContext.UsersRepo.GetByKeysAsync(authorsIds);
            var authorIdToNameMapping = authors.ToDictionary(u => u.Id, u => string.Format("{0} {1}", u.FirstName, u.LastName));
            var authorIdToUsernameMapping = authors.ToDictionary(u => u.Id, u => u.UserName);

            return groupUsers.Select(m =>
            {
                var contract = m.ToContract();
                contract.ParticipantName = authorIdToNameMapping[m.UserId];
                contract.ParticipantId = m.UserId;

                return contract;
            }).ToList();
        }

        [HttpPost]
        [Route("{groupId:int}/users/{userId:int}/files")]
        [AuthenticationTokenFilter]
        public async Task<GroupCreation> AttachFile(int groupId, int userId, GroupCreation groupCreation)
        {
            GroupUserEntity userGroup = await DbContext.GroupsUsersRepo.GetByUserIdAndGroupId(userId, groupId);

            if (userGroup == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Access is denied.");
            }

            ValidateUserAsAuthenticated(userGroup.UserId);

            CreationEntity creation = await DbContext.CreationsRepo.GetSingleOrDefaultAsync(groupCreation.CreationId);

            if (creation == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("Creation with id {0} not found.", groupCreation.CreationId));
            }

            GroupCreationEntity groupCreationEntity = await DbContext.GroupCreationsRepo.CreateAsync(
                new GroupCreationEntity()
                {
                    CreationId = creation.Id,
                    GroupId = groupCreation.GroupId,
                }).ConfigureAwait(false);

            return groupCreationEntity.ToContract();
        }

        [HttpGet]
        [Route("{groupId:int}/files")]
        [AuthenticationTokenFilter]
        public async Task<IEnumerable<Creation>> GetFilesInGroup(int groupId)
        {
            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId).ConfigureAwait(false);

            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("Group with id {0} not found.", groupId));
            }

            IEnumerable<CreationEntity> creations = await DbContext.GroupCreationsRepo.GetGroupCreations(groupId).ConfigureAwait(false);

            return creations.Select(c => c.ToContract()).ToList();
        }

        [HttpPost]
        [Route("{groupId:int}/users/{userId:int}/messages")]
        [AuthenticationTokenFilter]
        public async Task<GroupMessage> CreateMessageInGroup(int groupId, int userId, [FromBody]GroupMessage message)
        {
            GroupUserEntity userGroup = await DbContext.GroupsUsersRepo.GetByUserIdAndGroupId(userId, groupId);

            if (userGroup == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Access is denied.");
            }
            ValidateUserAsAuthenticated(userId);

            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId);

            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("Group with id {0} was not found.", groupId));
            }

            GroupMessageEntity entity = await DbContext.GroupMessagesRepo.CreateAsync(message.ToEntity());
            return entity.ToContract();
        }
    }
}