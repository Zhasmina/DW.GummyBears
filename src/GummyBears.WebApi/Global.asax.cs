using GummyBears.Repository;
using GummyBears.WebApi.App_Start;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GummyBears.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(DependencyInjection.ConfigureContainer);

            var dbContext = (IDbContext)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IDbContext));
            var tokenLifespan = new TimeSpan(0, Int32.Parse(ConfigurationManager.AppSettings["tokenLifespan_minutes"]), 0);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new TokenValidationHandler(dbContext, tokenLifespan));
        }
    }
}
