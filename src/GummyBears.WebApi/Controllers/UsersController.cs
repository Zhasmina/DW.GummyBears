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
        private IDbContext _dbContext;
        public UsersController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost, Route("")]
        [AllowAnonymous]
        public async Task<UserProfile> CreateUser(User user)
        {
            UserEntity userByUserName = await _dbContext.UsersRepo.GetByUserName(user.UserName);

            if (userByUserName != null)
            {
                ThrowHttpResponseException(HttpStatusCode.BadRequest, string.Format("User with name '{0}' already exists.", user.UserName));
            }

            UserEntity userByEmail = await _dbContext.UsersRepo.GetByEmail(user.Email);

            if (userByEmail != null)
            {
                ThrowHttpResponseException(HttpStatusCode.BadRequest, string.Format("User with email '{0}' already exists.", user.Email));
            }

            UserEntity createdUser = await _dbContext.UsersRepo.CreateAsync(user.ToEntity());

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
                UserEntity userEntity = await _dbContext.UsersRepo.GetSingleOrDefaultAsync(user.Id);

                if (userEntity.UserName != user.UserName)
                {
                    ThrowHttpResponseException(HttpStatusCode.BadRequest, "User name change is not allowed.");
                }

                if (userEntity.Email != user.Email)
                {
                    ThrowHttpResponseException(HttpStatusCode.BadRequest, "Email change is not allowed.");
                }

               await _dbContext.UsersRepo.UpdateAsync(user.ToEntity());

               updatedUser = await _dbContext.UsersRepo.GetSingleOrDefaultAsync(user.Id);

                transactionScope.Complete();
            }

            return updatedUser.ToModel();
        }

        [HttpGet, Route("{userId:int}")]
        [Authorize(Roles = "User")]
        public async Task<UserProfile> GetUser(int userId)
        {
           UserEntity userEntity = await _dbContext.UsersRepo.GetSingleOrDefaultAsync(userId);

            if(userEntity == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with id '{0}' not found.", userId));
            }


            return userEntity.ToModel();
        }
    }
}