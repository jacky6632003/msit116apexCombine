using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(msit116apexLayout.Startup))]
namespace msit116apexLayout
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
