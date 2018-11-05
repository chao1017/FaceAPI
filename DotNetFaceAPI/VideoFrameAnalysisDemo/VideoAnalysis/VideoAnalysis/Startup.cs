using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VideoAnalysis.Startup))]
namespace VideoAnalysis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
