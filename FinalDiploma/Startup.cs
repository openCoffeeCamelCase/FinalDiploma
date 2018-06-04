using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalDiploma.Startup))]
namespace FinalDiploma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
