using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using GummyBears.Repository;

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
                Content = new StringContent(message)
            };

            throw new HttpResponseException(response);
        }
    }
}