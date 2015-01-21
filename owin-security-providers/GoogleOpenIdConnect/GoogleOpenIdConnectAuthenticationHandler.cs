using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace owin_security_providers.GoogleOpenIdConnect
{
    public class GoogleOpenIdConnectAuthenticationHandler : AuthenticationHandler<GoogleOpenIdConnectAuthenticationOptions>
    {
        private const string TokenEndpoint = "https://accounts.google.com/o/oauth2/token";

        private readonly HttpClient httpClient;
        private readonly ILogger logger;

       
        public GoogleOpenIdConnectAuthenticationHandler(HttpClient httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }
        
        protected override async Task<Microsoft.Owin.Security.AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = null;

            try
            {
                string code = null;
                string state = null;

                IReadableStringCollection query = Request.Query;
                IList<string> values = query.GetValues("code");
                if (values != null && values.Count == 1)
                {
                    code = values[0];
                }
                values = query.GetValues("state");
                if (values != null && values.Count == 1)
                {
                    state = values[0];
                }

                properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return null;
                }

                // OAuth2 10.12 CSRF
                if (!ValidateCorrelationId(properties, logger))
                {
                    return new AuthenticationTicket(null, properties);
                }

                string requestPrefix = Request.Scheme + "://" + Request.Host;
                string redirectUri = requestPrefix + Request.PathBase + Options.CallbackPath;

                var body = new List<KeyValuePair<string, string>>();
                body.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
                body.Add(new KeyValuePair<string, string>("client_id", Options.ClientId));
                body.Add(new KeyValuePair<string, string>("client_secret", Options.ClientSecret));
                body.Add(new KeyValuePair<string, string>("redirect_uri", redirectUri));
                body.Add(new KeyValuePair<string, string>("code", code));
                HttpResponseMessage tokenResponse =
                  await httpClient.PostAsync(TokenEndpoint, new FormUrlEncodedContent(body));
                tokenResponse.EnsureSuccessStatusCode();
                string text = await tokenResponse.Content.ReadAsStringAsync();

                logger.WriteInformation(string.Format("POST response: {0}", text));


                JObject jsonResponse = JObject.Parse(text);
                if (jsonResponse["id_token"] != null)
                {
                    string token = jsonResponse["id_token"].ToString();
                    logger.WriteInformation(string.Format("Response id_token:{0}", token));
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtSecurityToken = (JwtSecurityToken) handler.ReadToken(token);
                    logger.WriteInformation(string.Format("JwtSecurityToken: {0}", jwtSecurityToken.ToString()));
                    if (jwtSecurityToken.Claims.Any(x => x.Type.Equals("email")))
                    {
                        Claim emailClaim = jwtSecurityToken.Claims.Where(x => x.Type.Equals("email")).First();
                        var identity = new ClaimsIdentity(Options.SignInAsAuthenticationType);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, emailClaim.Value, null, Options.AuthenticationType));
                        logger.WriteInformation(string.Format("Identifier {0}", emailClaim.Value));
                        return new AuthenticationTicket(identity, properties);
                    }
                }
                return new AuthenticationTicket(null, properties);
            }
            catch (Exception exception)
            {
                logger.WriteError(exception.Message);
                return new AuthenticationTicket(null, properties);
            }
           
        }
        
        protected override Task ApplyResponseChallengeAsync()
        {
            if (!Response.StatusCode.Equals(401))
                return Task.FromResult<object>(null);

            AuthenticationResponseChallenge challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (challenge != null)
            {
                string baseUri =
                    Request.Scheme +
                    Uri.SchemeDelimiter +
                    Request.Host +
                    Request.PathBase;

                string currentUri =
                    baseUri +
                    Request.Path +
                    Request.QueryString;

                string redirectUri =
                    baseUri +
                    Options.CallbackPath;

                AuthenticationProperties properties = challenge.Properties;
                if (string.IsNullOrEmpty(properties.RedirectUri))
                {
                    properties.RedirectUri = currentUri;
                }

                // OAuth2 10.12 CSRF
                GenerateCorrelationId(properties);

                // comma separated
                string scope = string.Join(" ", Options.Scope);

                string state = Options.StateDataFormat.Protect(properties);


                string authorizationEndpoint =
                    "https://accounts.google.com/o/oauth2/auth" +
                    "?response_type=code" +
                    "&client_id=" + Uri.EscapeDataString(Options.ClientId) +
                    "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
                    "&scope=" + Uri.EscapeDataString(scope) +
                    "&state=" + Uri.EscapeDataString(state);
                ;
            
                Response.Redirect(authorizationEndpoint);
            }

            return Task.FromResult<object>(null);
        }
        
        public override async Task<bool> InvokeAsync()
        {
 	        if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
            {
                var ticket = await AuthenticateAsync();

                if (ticket != null)
                {
                    Context.Authentication.SignIn(ticket.Properties, ticket.Identity);

                    Response.Redirect(ticket.Properties.RedirectUri);

                    // Prevent further processing by the owin pipeline.
                    return true;
                }
            }
            // Let the rest of the pipeline run.
            return false;
        }
    
    }
}
