using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace owin_security_providers.MobileOpenIdConnect
{
    public static class MobileOpenIdConnectAuthenticationExtensions
    {
        public static IAppBuilder UseMobileOpenIdConnectAuthentication
            (this IAppBuilder app,
             MobileOpenIdConnectAuthenticationOptions options)
        {
            return app.Use(typeof(MobileOpenIdConnectAuthenticationMiddleware), app, options);
        }
    }
}
