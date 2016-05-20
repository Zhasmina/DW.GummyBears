using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using GummyBears.Clients;

namespace GummyBears.Web.App_Start
{
    public class DependencyInjection
    {
        public static void ConfigureContainer()














        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.Register(context => new GummyBearClient(new HttpClient(), new DefaultGummyBearsClientSettings())).As<IGummyBearClient>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}