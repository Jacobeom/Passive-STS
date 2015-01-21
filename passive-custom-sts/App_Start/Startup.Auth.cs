using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Twitter;
using Owin;
using owin_security_providers.GoogleOpenIdConnect;
using owin_security_providers.MobileOpenIdConnect;
using owin_security_providers.LinkedIn;
using  owin_security_providers.GitHub;
using System;

namespace passive_custom_sts
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(5),
            };
            app.UseCookieAuthentication(cookieAuthenticationOptions);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

           
            GoogleOpenIdConnectAuthenticationOptions googleOptions = new GoogleOpenIdConnectAuthenticationOptions(
                "xxxxxxxxxxxxxxxxxx",
                "xxxxxxxxxxxxxxxx");
            app.UseGoogleOpenIdConnectAuthentication(googleOptions);

            MobileOpenIdConnectAuthenticationOptions mobileOptions = new MobileOpenIdConnectAuthenticationOptions(
                "xxxx",
                "xxxx");
            app.UseMobileOpenIdConnectAuthentication(mobileOptions);

            TwitterAuthenticationOptions twitterOptions = new TwitterAuthenticationOptions();
            twitterOptions.ConsumerKey = "xxxxx";
            twitterOptions.ConsumerSecret = "xxxxx";
            app.UseTwitterAuthentication(twitterOptions);

            LinkedInAuthenticationOptions linkedinOptions= new LinkedInAuthenticationOptions();
            linkedinOptions.ClientId = "xxxxx";
            linkedinOptions.ClientSecret = "xxxxx";
            app.UseLinkedInAuthentication(linkedinOptions);

            GitHubAuthenticationOptions gitOptions= new GitHubAuthenticationOptions();
            gitOptions.ClientId ="xxxxxx";
            gitOptions.ClientSecret ="xxxxx";
            app.UseGitHubAuthentication(gitOptions);
        }
    }
}