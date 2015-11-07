using GummyBears.Repository;
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
        [Route("groupId:int}")]
        [Authorize(Roles = "User")]
        public async Task<IEnumerable<GroupMessageEntity>> GetMessagesInGroup([FromUri]int groupId)
        {
            AuthenticationEntity authentication = await DbContext.AuthenticationRepo.GetSingleOrDefaultAsync(AuthenticationToken);

            GroupUserEntity userGroup = await DbContext.GroupsUsersRepo.GetByUserIdAndGroupId(authentication.UserId, groupId);

            if (userGroup == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Access is denied.");
            }

            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId);

            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("Group with id {0} was not found.", groupId));
            }

            IEnumerable<GroupMessageEntity> messages = await DbContext.GroupMessagesRepo.GetGroupMessages(groupId);

            return messages;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "User")]
        public async Task<Group> CreateGroup([FromBody]Group group)
        {
            AuthenticationEntity authentication = await DbContext.AuthenticationRepo.GetSingleOrDefaultAsync(AuthenticationToken);
            
            if (authentication == null || authentication.UserId != group.AuthorId)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Wrong authentication token.");
            }

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var createdGroup = await DbContext.GroupsRepo.CreateAsync(group.ToEntity());
                await DbContext.GroupsUsersRepo.CreateAsync(new GroupUserEntity
                {
                    GroupId = createdGroup.Id,
                    UserId = authentication.UserId,
                    IsAdmin = true
                });

                transactionScope.Complete();
            }

            return group;
        }

        //TODO not finished yet
        [HttpPost]
        [Route("{groupId:int}/participants")]
        [Authorize(Roles = "User")]
        public async Task<GroupParticipants> AddParticipants(int groupId, [FromBody]GroupParticipants groupParticipants) 
        {
            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId);

            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.BadRequest, string.Format("Group with id {0} not exists.", groupId));
            }

            AuthenticationEntity authentication = await DbContext.AuthenticationRepo.GetSingleOrDefaultAsync(AuthenticationToken);

            if (authentication == null || authentication.UserId != group.AuthorId)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Wrong authentication token.");
            }

            //DbContext.GroupsUsersRepo

            return null;
        }
    }
}