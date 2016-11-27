using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(disruptappService.Startup))]

namespace disruptappService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}