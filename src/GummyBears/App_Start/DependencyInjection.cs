using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Autofac;
using GummyBears.Clients;
using System.Net.Http;

namespace GummyBears.App_Start
{
    public class DependencyInjection
    {
        public static void ConfigureContainer(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();
             builder.Register(c => 
                 new GummyBearClient(new System.Net.Http.HttpMessageInvoker(), new DefaultGummyBearsClientSettings())).As<IGummyBearClient>();
            
        }
    }
}