using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using GummyBears.Repository;
using Dapperer.DbFactories;
using Dapperer.QueryBuilders.MsSql;
using Dapperer;
using GummyBears.WebApi.Controllers;
using GummyBears.Common;
using GummyBears.WebApi.Filters;

namespace GummyBears.WebApi.App_Start
{
    public class DependencyInjection
    {
        public static void ConfigureContainer(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(BaseController).Assembly);
            builder.RegisterType<ModelValidationFilter>().As<ModelValidationFilter>();
            builder.RegisterType<ModelValidationFilter>().AsWebApiActionFilterFor<BaseController>();
            builder.RegisterType<ApiErrorFilter>().As<ApiErrorFilter>();
            builder.RegisterType<ApiErrorFilter>().AsWebApiExceptionFilterFor<ApiController>();

            // Repository
            builder.RegisterType<SqlQueryBuilder>().As<IQueryBuilder>().SingleInstance();
            builder.RegisterType<SqlDbFactory>().As<IDbFactory>();
            builder.RegisterType<DappererSettings>().As<IDappererSettings>();
            builder.RegisterType<DbContext>().As<IDbContext>();

            builder.RegisterType<TokenGenerator>().As<ITokenGenerator>();
            builder.RegisterType<CreationRightsManager.Manager>();

            var container = builder.Build();

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}