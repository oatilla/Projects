using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HaberPortal.Startup))]
namespace HaberPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
