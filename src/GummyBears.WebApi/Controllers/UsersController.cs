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
            :base(dbContext)
        {
        }

        [HttpPost, Route("")]
        [AllowAnonymous]
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

            return createdUser.ToModel();
        }

        [HttpPut, Route("{userId:int}")]
        [Authorize(Roles = "User")]
        public async Task<UserProfile> UpdateUser(int userId, [FromBody]User user)
        {
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

            return updatedUser.ToModel();
        }

        [HttpGet, Route("{userId:int}")]
        [Authorize(Roles = "User")]
        public async Task<UserProfile> GetUser(int userId)
        {
            UserEntity userEntity = await DbContext.UsersRepo.GetSingleOrDefaultAsync(userId);

            if(userEntity == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with id '{0}' not found.", userId));
            }


            return userEntity.ToModel();
        }

        [HttpGet, Route("{userId}/groups")]
        [Authorize(Roles = "User")]
        public async Task<List<GroupEntity>> GetAllUserGroups([FromUri]int userId)
        {
           UserEntity user = await DbContext.UsersRepo.GetSingleOrDefaultAsync(userId);
           if (user == null)
           {
               ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with id '{0}' not found.", userId));
           }

           IEnumerable<GroupEntity> groups = await DbContext.GroupsRepo.GetUserGroups(userId);

            return groups.ToList();
        }
    }
}