using GummyBears.Contracts;
using GummyBears.Repository;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using GummyBears.WebApi.Helpers;
using System.Transactions;
using System.Net.Http;
using System.Net;

namespace GummyBears.WebApi.Controllers
{
    [System.Web.Http.RoutePrefix("users")]
    public class UsersController : BaseController
    {
        public UsersController(IDbContext dbContext)
            : base(dbContext)
        {
        }

        [HttpPost, Route("")]
        public async Task<UserProfile> CreateUser(User user)
        {
            UserEntity userByUserName = await DbContext.UsersRepo.GetByUserName(user.UserName);

            if (userByUserName != null)
            {
                ThrowHttpResponseException(HttpStatusCode.BadRequest, string.Format("User with name '{0}' already exists.", user.UserName));
            }

            UserEntity userByEmail = await DbContext.UsersRepo.GetByEmail(user.Email);

            if (userByEmail != null)
            {
                ThrowHttpResponseException(HttpStatusCode.BadRequest, string.Format("User with email '{0}' already exists.", user.Email));
            }

            UserEntity createdUser = await DbContext.UsersRepo.CreateAsync(user.ToEntity());

            return createdUser.ToStrippedModel();
        }

        [HttpPut, Route("{userId:int}")]
        [AuthenticationTokenFilter]
        public async Task<UserProfile> UpdateUser(int userId, [FromBody]User user)
        {
            ValidateUserAsAuthenticated(userId);
            if (user.Id != 0 && user.Id != userId)
            {
                ThrowHttpResponseException(HttpStatusCode.BadRequest, "User Id change is not allowed.");
            }

            user.Id = userId;

            UserEntity updatedUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UserEntity userEntity = await DbContext.UsersRepo.GetSingleOrDefaultAsync(user.Id);

                if (userEntity.UserName != user.UserName)
                {
                    ThrowHttpResponseException(HttpStatusCode.BadRequest, "User name change is not allowed.");
                }

                if (userEntity.Email != user.Email)
                {
                    ThrowHttpResponseException(HttpStatusCode.BadRequest, "Email change is not allowed.");
                }

                await DbContext.UsersRepo.UpdateAsync(user.ToEntity());

                updatedUser = await DbContext.UsersRepo.GetSingleOrDefaultAsync(user.Id);

                transactionScope.Complete();
            }

            return updatedUser.ToStrippedModel();
        }

        [HttpGet, Route("{userId:int}")]
        [AuthenticationTokenFilter]
        public async Task<UserProfile> GetUser(int userId)
        {
            UserEntity userEntity = await DbContext.UsersRepo.GetSingleOrDefaultAsync(userId);

            if (userEntity == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with id '{0}' not found.", userId));
            }

            return userEntity.ToStrippedModel();
        }

        [HttpGet, Route("{username}", Order = 1)]
        [AuthenticationTokenFilter]
        public async Task<UserProfile> GetUser(string username)
        {
            UserEntity userEntity = await DbContext.UsersRepo.GetByUserName(username);

            if (userEntity == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with username '{0}' not found.", username));
            }

            return userEntity.ToStrippedModel();
        }

        [HttpGet, Route("")]
        [AuthenticationTokenFilter]
        public async Task<List<UserProfileBrief>> GetAllUsers()
        {
            var users = await DbContext.UsersRepo.GetAllAsync();

            return users.Select(u => u.ToBriefModel()).ToList();
        }

        [HttpGet, Route("{userId}/groups")]
        [AuthenticationTokenFilter]
        public async Task<List<Group>> GetAllUserGroups([FromUri]int userId)
        {
            UserEntity user = await DbContext.UsersRepo.GetSingleOrDefaultAsync(userId);
            if (user == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with id '{0}' not found.", userId));
            }

            IEnumerable<GroupEntity> groups = await DbContext.GroupsRepo.GetUserGroups(userId);

            return groups.Select(g => g.ToContract()).ToList();
        }
    }
}