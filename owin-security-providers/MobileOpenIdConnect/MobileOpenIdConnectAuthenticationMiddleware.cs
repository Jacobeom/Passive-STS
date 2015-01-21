using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.DataHandler;
using System.Net.Http;
using Microsoft.Owin.Logging;

namespace owin_security_providers.MobileOpenIdConnect
{
    public class MobileOpenIdConnectAuthenticationMiddleware : AuthenticationMiddleware<MobileOpenIdConnectAuthenticationOptions>
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public MobileOpenIdConnectAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, MobileOpenIdConnectAuthenticationOptions options)
            : base(next, options)
        { 
            if(string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            }
            if (Options.StateDataFormat == null)
            {
                IDataProtector dataProtector = app.CreateDataProtector(
                    typeof(MobileOpenIdConnectAuthenticationOptions).FullName,
                    Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            logger = app.CreateLogger<MobileOpenIdConnectAuthenticationMiddleware>();

            if (String.IsNullOrEmpty(Options.SignInAsAuthenticationType))
                Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();

            httpClient = new HttpClient(new WebRequestHandler())
            {
                Timeout = Options.BackchannelTimeout,
                MaxResponseContentBufferSize = 1024 * 1024 * 10
            };
        }


        protected override AuthenticationHandler<MobileOpenIdConnectAuthenticationOptions> CreateHandler()
        {
            return new MobileOpenIdConnectAuthenticationHandler(httpClient, logger);
        }

       
    }
}
