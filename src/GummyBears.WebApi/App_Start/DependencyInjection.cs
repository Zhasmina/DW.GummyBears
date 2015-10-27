using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Dapperer.QueryBuilders.MsSql;
using Dapperer.DbFactories;
using GummyBears.Repository;
using Dapperer;

namespace GummyBears.WebApi.App_Start
{
    public class DependencyInjection
    {
        public static void ConfigureContainer(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            var container = builder.Build();

            // Repository
            builder.RegisterType<SqlQueryBuilder>().As<IQueryBuilder>().SingleInstance();
            builder.RegisterType<SqlDbFactory>().As<IDbFactory>();
            builder.RegisterType<DappererSettings>().As<IDappererSettings>();
            builder.RegisterType<DbContext>().As<IDbContext>();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}