using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicStoree.Startup))]
namespace MusicStoree
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
