using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Labb2_Dis.Startup))]
namespace Labb2_Dis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
