using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Checkerboard.Startup))]
namespace Checkerboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
