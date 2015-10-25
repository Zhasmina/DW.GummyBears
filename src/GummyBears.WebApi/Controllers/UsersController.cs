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
        public async Task<User> CreateUser(User user)
        {
            UserEntity userByUserName = await _dbContext.UsersRepo.GetByUserName(user.UserName);

            if (userByUserName != null)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("User with name '{0}' already exists.", user.UserName)),
                    ReasonPhrase = string.Format("User with name '{0}' already exists.", user.UserName)
                };

                throw new HttpResponseException(responseMessage);
            }

            UserEntity userByEmail = await _dbContext.UsersRepo.GetByEmail(user.Email);

            if (userByEmail != null)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("User with name '{0}' already exists.", user.UserName)),
                    ReasonPhrase = string.Format("User with name '{0}' already exists.", user.UserName)
                };

                throw new HttpResponseException(responseMessage);
            }

            UserEntity createdUser = await _dbContext.UsersRepo.CreateAsync(user.ToEntity());

            return createdUser.ToModel();
        }

        [HttpPut, Route("{userId:int}")]
        [Authorize(Roles = "User")]
        public async Task<User> UpdateUser(int userId, [FromBody]User user)
        {
            if (user.Id != 0 && user.Id != userId)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("User Id change is not allowed."),
                    ReasonPhrase = "User Id change is not allowed."
                };

                throw new HttpResponseException(responseMessage);
            }

            user.Id = userId;

            UserEntity updatedUser;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UserEntity userEntity = await _dbContext.UsersRepo.GetSingleOrDefaultAsync(user.Id);

                if (userEntity.UserName != user.UserName)
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("User name change is not allowed."),
                        ReasonPhrase = "User name change is not allowed."
                    };

                    throw new HttpResponseException(responseMessage);
                }

                if (userEntity.Email != user.Email)
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Email change is not allowed."),
                        ReasonPhrase = "Email change is not allowed."
                    };

                    throw new HttpResponseException(responseMessage);
                }

               await _dbContext.UsersRepo.UpdateAsync(user.ToEntity());

               updatedUser = await _dbContext.UsersRepo.GetSingleOrDefaultAsync(user.Id);

                transactionScope.Complete();
            }

            return updatedUser.ToModel();
        }

        [HttpGet, Route("{userId:int}")]
        public async Task<IHttpActionResult> GetUser(int userId)
        {
            return null;
        }

        [HttpPost, Route("login")]
        public IHttpActionResult Login()
        {
            return null;
        }
    }
}