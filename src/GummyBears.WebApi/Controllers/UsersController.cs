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

namespace GummyBears.WebApi.Controllers
{
    [System.Web.Http.RoutePrefix("v1/users")]
    public class UsersController : BaseController
    {
        private IDbContext _dbContext;
        public UsersController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> CreateUser(User user)
        {
            UserEntity userByUserName = await _dbContext.UsersRepo.GetByUserName(user.UserName);

            if (userByUserName != null)
            {
                return BadRequest(string.Format("User with name '{0}' already exists.", user.UserName));
            }

            UserEntity userByEmail = await _dbContext.UsersRepo.GetByEmail(user.Email);

            if (userByEmail != null)
            {
                return BadRequest(string.Format("User with email '{0}' already exists.", user.Email));
            }

            UserEntity createdUser = await _dbContext.UsersRepo.CreateAsync(user.ToEntity());

            return Ok(createdUser.ToModel());
        }

        [HttpPut, Route("{userId:int}")]
        [Authorize(Roles = "User")]
        public async Task<IHttpActionResult> UpdateUser(int userId, [FromBody]User user)
        {
            if (user.Id != 0 && user.Id != userId)
            {
                return BadRequest("User id change is not allowed.");
            }

            user.Id = userId;

            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                UserEntity userEntity = await _dbContext.UsersRepo.GetSingleOrDefaultAsync(user.Id);

                if (userEntity.UserName != user.UserName)
                {
                    return BadRequest("User name change is not allowed.");
                }

                if (userEntity.Email != user.Email)
                {
                    return BadRequest("Email change is not allowed.");
                }

                await _dbContext.UsersRepo.UpdateAsync(user.ToEntity());

                transactionScope.Complete();
            }

            return Ok();
        }

        [HttpPost, Route("login")]
        public IHttpActionResult Login()
        {
            return null;
        }
    }
}