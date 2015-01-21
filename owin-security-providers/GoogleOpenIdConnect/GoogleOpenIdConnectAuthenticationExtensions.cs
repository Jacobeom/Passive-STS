using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace owin_security_providers.GoogleOpenIdConnect
{
    public static class GoogleOpenIdConnectAuthenticationExtensions
    {
        public static IAppBuilder UseGoogleOpenIdConnectAuthentication
            (this IAppBuilder app, 
             GoogleOpenIdConnectAuthenticationOptions options)
        {
            return app.Use(typeof(GoogleOpenIdConnectAuthenticationMiddleware), app, options);
        }
    }
}
