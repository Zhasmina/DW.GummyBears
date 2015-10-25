using GummyBears.Common;
using GummyBears.Contracts;
using GummyBears.Entities;
using GummyBears.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace GummyBears.WebApi.Controllers
{
    [System.Web.Http.RoutePrefix("usercredentials")]
    public class CredentialsController : BaseController
    {
        private IDbContext _dbContext;
        private ITokenGenerator _tokenGenerator;

        public CredentialsController(IDbContext dbContext, ITokenGenerator tokenGenerator)
        {
            _dbContext = dbContext;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [Route("")]
        public async Task<AuthenticationData> Login([FromBody]Credentials credentials)
        {
            UserEntity user = await _dbContext.UsersRepo.GetByUserName(credentials.Username);
            if (user == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("User with name '{0}' not found.", credentials.Username));
            }

            if (!user.Password.Equals(credentials.Password))
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Wrong password");
            }

            string token = _tokenGenerator.GenerateToken();
            await _dbContext.AuthenticationRepo.CreateAsync(new AuthenticationEntity()
            {
                Token = token,
                LastSeen = DateTime.UtcNow,
                UserId = user.Id
            });

            return new AuthenticationData
            {
                Token = token,
                UserId = user.Id,
                Username = user.UserName
            };
        }

    }
}