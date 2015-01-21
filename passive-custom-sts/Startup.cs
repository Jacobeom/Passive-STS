using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Extensions;

[assembly: OwinStartupAttribute(typeof(passive_custom_sts.Startup))]
namespace passive_custom_sts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetLoggerFactory(new Logging.CommonLoggingLoggerFactory());

            ConfigureAuth(app);
        }
    }
}
