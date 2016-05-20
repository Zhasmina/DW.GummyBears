using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using System.Web.Http;
using GummyBears.Contracts;

namespace GummyBears.WebApi.Filters
{
    public class ApiErrorFilter : ExceptionFilterAttribute, IAutofacExceptionFilter
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;
            if (context.Exception is HttpResponseException)
            {
               // httpStatusCode = (context.Exception as HttpResponseException)..StatusCode;
            }

            context.Response = context.Request.CreateResponse(httpStatusCode, new[] { new Error(context.Exception.Message) });
        }
    }
}