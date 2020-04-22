using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EcommsPortal.Startup))]
namespace EcommsPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
