using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using GummyBears.WebApi.Controllers;
using GummyBears.WebApi.Helpers;
using GummyBears.Entities;
using GummyBears.Repository;
using System.Web.Http;
using System.Linq;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net;

namespace GummyBears.WebApi
{
    public class AuthenticationTokenFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IDbContext dbContext = (IDbContext)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IDbContext));
            TimeSpan tokenLifespan = new TimeSpan(0, Int32.Parse(ConfigurationManager.AppSettings["tokenLifespan_minutes"]), 0);

            if (actionContext.Request.Headers == null ||
                !actionContext.Request.Headers.Contains("Authorization-Token") ||
                string.IsNullOrEmpty(actionContext.Request.Headers.GetValues("Authorization-Token").FirstOrDefault()))

            {
                throw new HttpResponseException(actionContext.Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            string token = actionContext.Request.Headers.GetValues("Authorization-Token").FirstOrDefault();
            AuthenticationEntity authentication = dbContext.AuthenticationRepo.GetSingleOrDefault(token);
            if (authentication == null || (authentication.LastSeen.Add(tokenLifespan) < DateTime.UtcNow))
            {
                throw new HttpResponseException(actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            authentication.LastSeen = DateTime.UtcNow;
            dbContext.AuthenticationRepo.Update(authentication);

            UserEntity user = dbContext.UsersRepo.GetSingleOrDefault(authentication.UserId);

            if (user == null)
            {
                throw new HttpResponseException(actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            actionContext.RequestContext.Principal = new SimplePrincipal(user.UserName, user.Id, "user");

            base.OnActionExecuting(actionContext);
        }
    }
}