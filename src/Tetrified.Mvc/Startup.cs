using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tetrified.Mvc.Startup))]
namespace Tetrified.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
