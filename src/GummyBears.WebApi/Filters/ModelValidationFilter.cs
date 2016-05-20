using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using Autofac;
using Autofac.Integration.WebApi;
using GummyBears.Contracts;

namespace GummyBears.WebApi.Filters
{
    public class ModelValidationFilter : ActionFilterAttribute, IAutofacActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requestLifetimeScope = actionContext.Request.GetDependencyScope().GetRequestLifetimeScope();

            var modelState = actionContext.ModelState;

            if (!modelState.IsValid)
            {
                Error[] errors = GetErrors(actionContext.ModelState).ToArray();


                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }

        private IEnumerable<Error> GetErrors(ModelStateDictionary modelState)
        {
            foreach (string key in modelState.Keys)
            {
                foreach (ModelError modelError in modelState[key].Errors)
                {
                    string errorMessage = modelError.ErrorMessage;

                    if (string.IsNullOrWhiteSpace(errorMessage) && modelError.Exception != null)
                    {
                        errorMessage = modelError.Exception.Message;
                    }

                    yield return new Error(string.Format("[{0}] {1}", key.Substring(key.IndexOf('.') + 1), errorMessage));
                }
            }
        }
    }
}