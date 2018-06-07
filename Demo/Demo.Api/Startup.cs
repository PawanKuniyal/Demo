using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Helpa.Api.Startup))]

namespace Helpa.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {            
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
