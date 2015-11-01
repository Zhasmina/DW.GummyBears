using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using GummyBears.WebApi.Controllers;
using GummyBears.WebApi.Helpers;

namespace GummyBears.WebApi
{
    public class AuthenticationTokenFilter : ActionFilterAttribute, IAutofacActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as BaseController;
            if (controller != null)
            {
                controller.SetAuthenticationToken(actionContext.Request.GetAuthorizationToken());
            }
        }
    }
}