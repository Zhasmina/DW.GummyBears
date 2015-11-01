using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GummyBears.Startup))]
namespace GummyBears
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
