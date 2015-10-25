using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace GummyBears.WebApi.App_Start
{
    public class DependencyInjection
    {
        public static void ConfigureContainer(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            var container = builder.Build();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}