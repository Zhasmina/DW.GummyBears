using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using GummyBears.Repository;
using GummyBears.WebApi.Filters;
using GummyBears.Contracts;

namespace GummyBears.WebApi.Controllers
{
    public class BaseController : ApiController 
    {
        protected IDbContext DbContext;

        protected string AuthenticationToken;
        public BaseController(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected void ThrowHttpResponseException(HttpStatusCode statusCode, string message)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<Error[]>(new[] { new Error(message)}, new System.Net.Http.Formatting.JsonMediaTypeFormatter())
            };

            throw new HttpResponseException(response);
        }

        protected void ValidateUserAsAuthenticated(int userId)
        {
            int authUserId = ((SimpleIdentity)((SimplePrincipal)ActionContext.RequestContext.Principal).Identity).Id;
            if (userId != authUserId)
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Logged as different user");
        }

        protected void ValidateUserAsAdmin()
        {
            bool isAdmin = ((SimplePrincipal)ActionContext.RequestContext.Principal).IsInRole("admin");
            if (!isAdmin)
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Not logged as admin");
        }
    }
}