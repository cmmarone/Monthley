using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Monthley.WebMVC.Startup))]
namespace Monthley.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
