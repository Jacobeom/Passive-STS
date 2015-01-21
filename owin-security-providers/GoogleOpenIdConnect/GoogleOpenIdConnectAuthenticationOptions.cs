using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace owin_security_providers.GoogleOpenIdConnect
{
    public class GoogleOpenIdConnectAuthenticationOptions : AuthenticationOptions
    {
         public GoogleOpenIdConnectAuthenticationOptions(string clientId, string clientSecret)
            : base(Constants.DefaultAuthenticationType)
        {
            Description.Caption = Constants.DefaultAuthenticationType;
            CallbackPath = new PathString("/signin-google-id-connect");
            AuthenticationMode = AuthenticationMode.Passive;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scope = new List<string> { "email" };
            BackchannelTimeout = TimeSpan.FromSeconds(60);
        }

        public IList<string> Scope { get; private set; }
       
        public PathString CallbackPath { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string SignInAsAuthenticationType { get; set; }

        public TimeSpan BackchannelTimeout { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

    }
}
