using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RavenDemoMvc.Startup))]
namespace RavenDemoMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
