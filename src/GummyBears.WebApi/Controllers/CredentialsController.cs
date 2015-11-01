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
        private ITokenGenerator _tokenGenerator;

        public CredentialsController(IDbContext dbContext, ITokenGenerator tokenGenerator)
            : base(dbContext)
        {
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [Route("")]
        public async Task<AuthenticationData> Login([FromBody]Credentials credentials)
        {
            UserEntity user = await DbContext.UsersRepo.GetByUserName(credentials.Username);
            if (user == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("User with name '{0}' not found.", credentials.Username));
            }

            if (!user.Password.Equals(credentials.Password))
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Wrong password");
            }

            string token = _tokenGenerator.GenerateToken();
            await DbContext.AuthenticationRepo.CreateAsync(new AuthenticationEntity()
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

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "User")]
        public async Task<string> Logout()
        {
            AuthenticationEntity authentication = await DbContext.AuthenticationRepo.GetSingleOrDefaultAsync(AuthenticationToken);

            if (authentication == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Wrong authentication token.");
            }

            await DbContext.AuthenticationRepo.DeleteAsync(AuthenticationToken);

            return string.Format("Successful logout");
        }
    }
}