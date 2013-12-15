using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnLib.Startup))]
namespace OnLib
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
