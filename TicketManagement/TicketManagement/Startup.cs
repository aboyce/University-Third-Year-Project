using Microsoft.Owin;
using Owin;
using TicketManagement;

[assembly: OwinStartup(typeof(Startup))]
namespace TicketManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
