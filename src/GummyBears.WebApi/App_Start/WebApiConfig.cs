using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GummyBears.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new Filters.ApiErrorFilter());
            config.Filters.Add(new Filters.ModelValidationFilter());

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
