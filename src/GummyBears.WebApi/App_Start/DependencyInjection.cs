using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Http.Dependencies;
using GummyBears.Repository;
using Dapperer.DbFactories;
using Dapperer.QueryBuilders.MsSql;
using Dapperer;
using GummyBears.WebApi.Controllers;
using GummyBears.Common;

namespace GummyBears.WebApi.App_Start
{
    public class DependencyInjection
    {
        public static void ConfigureContainer(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(BaseController).Assembly);
            // Repository
            builder.RegisterType<SqlQueryBuilder>().As<IQueryBuilder>().SingleInstance();
            builder.RegisterType<SqlDbFactory>().As<IDbFactory>();
            builder.RegisterType<DappererSettings>().As<IDappererSettings>();
            builder.RegisterType<DbContext>().As<IDbContext>();

            builder.RegisterType<TokenGenerator>().As<ITokenGenerator>();

            var container = builder.Build();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}