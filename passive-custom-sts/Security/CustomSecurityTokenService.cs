using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web;


namespace passive_custom_sts.Security
{
    public class StsIdentity : System.Security.Principal.IIdentity
    {
        private string _authenticationType;
        private string _name;

        public StsIdentity(string name, string authType)
        {
            _name = name;
            _authenticationType = authType;
        }
        public string AuthenticationType
        {
            get { return _authenticationType; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
    public class CustomSecurityTokenService : SecurityTokenService
    {
        static readonly string[] SupportedWebApps = { };


        public CustomSecurityTokenService(SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
            : base(securityTokenServiceConfiguration)
        {
        }

        static void ValidateAppliesTo(EndpointReference appliesTo)
        {
            if (SupportedWebApps == null || SupportedWebApps.Length == 0) return;

            var validAppliesTo = SupportedWebApps.Any(x => appliesTo.Uri.Equals(x));

            if (!validAppliesTo)
            {
                throw new InvalidRequestException(String.Format("The 'appliesTo' address '{0}' is not valid.", appliesTo.Uri.OriginalString));
            }
        }

        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            ValidateAppliesTo(request.AppliesTo);

            var scope = new Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EncryptionCertificate"]))
            {
                // Important note on setting the encrypting credentials.
                // In a production deployment, you would need to select a certificate that is specific to the RP that is requesting the token.
                // You can examine the 'request' to obtain information to determine the certificate to use.

                var encryptingCertificate = GetCertificate(ConfigurationManager.AppSettings["EncryptionCertificate"]);
                var encryptingCredentials = new X509EncryptingCredentials(encryptingCertificate);
                scope.EncryptingCredentials = encryptingCredentials;
            }
            else
            {
                // If there is no encryption certificate specified, the STS will not perform encryption.
                // This will succeed for tokens that are created without keys (BearerTokens) or asymmetric keys.  
                scope.TokenEncryptionRequired = false;
            }

            if (scope.AppliesToAddress == "urn:tid.com:facebook")
            {
                scope.ReplyToAddress = "http://win-h57kvthlmko:6760/_trust/";
            }
            else if (scope.AppliesToAddress == "urn:dev.tid.com:facebook")
            {
                scope.ReplyToAddress = "https://des-is-sso.hi.inet/_trust/";
            }
            else if (scope.AppliesToAddress == "urn:dev.tid.com:openid")
            {
                scope.ReplyToAddress = "https://des-is-sso.hi.inet/_trust/";
            }
            else if (scope.AppliesToAddress == "urn:tid.com:dev-adfs")
            {
                scope.ReplyToAddress = "https://fed-srv-dev.hi.inet/adfs/ls/";
            }
            else
            {
                scope.ReplyToAddress = scope.AppliesToAddress;
            }
            return scope;
        }

        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {

            //List<Claim> result = new List<Claim>();

            //foreach (Claim claim in principal.Claims)
            //{
            //    if (claim.Type.Equals("http://schemas.xmlsoap.org/claims/CommonName") ||
            //        claim.Type.Equals(ClaimTypes.Email) || claim.Type.Equals(ClaimTypes.Upn))
            //    {
            //        result.Add(claim);
            //    }
            //}
            //System.Security.Principal.IIdentity id = new StsIdentity(principal.Identity.Name, principal.Identity.AuthenticationType);
            //var identity = new ClaimsIdentity(result);
            //return identity;
            var identity = new ClaimsIdentity(principal.Claims);
            return identity;
        }

        public static X509Certificate2 GetCertificate(string subjectName)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                certificates = store.Certificates;
                var certs = certificates.OfType<X509Certificate2>().Where(x => x.SubjectName.Name.Equals(subjectName, StringComparison.OrdinalIgnoreCase)).ToList();

                if (certs.Count == 0)
                    throw new ApplicationException(string.Format("No certificate was found for subject Name {0}", subjectName));
                else if (certs.Count > 1)
                    throw new ApplicationException(string.Format("There are multiple certificates for subject Name {0}", subjectName));

                return new X509Certificate2(certs[0]);
            }
            finally
            {
                if (certificates != null)
                {
                    for (var i = 0; i < certificates.Count; i++)
                    {
                        var cert = certificates[i];
                        cert.Reset();
                    }
                }
                store.Close();
            }
        }
    }
}