using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnnaAndSarahWeb.Startup))]
namespace AnnaAndSarahWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
