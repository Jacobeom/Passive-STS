using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using passive_custom_sts.Security;
using Microsoft.AspNet.Identity.Owin;
using aspnet_custom_identity.Service;
using aspnet_custom_identity.Model;
using passive_custom_sts.Models;

namespace passive_custom_sts.Controllers
{
    [Authorize]
    public class AccountController : AsyncController
    {
      
        public AccountController(CustomUserManager userManager, IActiveDirectoryService activeDirectoryService)
        {
            UserManager = userManager;
            ActiveDirectoryService = activeDirectoryService;
        }

        public CustomUserManager UserManager { get; private set; }
        public IActiveDirectoryService ActiveDirectoryService { get; private set; }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
       
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl}));
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo != null)
            {
                string externalProviderKey = loginInfo.Login.ProviderKey;
                string externalProvider = loginInfo.Login.LoginProvider;

                var user = await UserManager.FindByIdAsync(new UserId(externalProviderKey,GetTypeOfExternalProvider(externalProvider)));
                if (user != null)
                {
                    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                    return Redirect(returnUrl);
                }
            }
            return new RedirectResult(Url.Action("AssociateAccount", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult AssociateAccount(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> AssociateAccount(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (loginInfo!= null)
                {
                    string loginKey = loginInfo.Login.ProviderKey;
                    string externalProvider = loginInfo.Login.LoginProvider;

                    if(ActiveDirectoryService.Authenticate(model.UserName, model.Password))
                    {
                        await SignInAsync(model.UserName, loginKey, externalProvider);
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login account or password.");
                        ViewBag.ReturnUrl = returnUrl;
                        return View(model);
                    }
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            return View(model);
        }

        private Type GetTypeOfExternalProvider(string externalProvider)
        {
            if (externalProvider.Equals("GoogleOpenIdConnect"))
                return typeof(GoogleUser);
            else if (externalProvider.Equals("Twitter"))
                return typeof(TwitterUser);
            else if (externalProvider.Equals("LinkedIn"))
                return typeof(LinkedInUser);
            else if (externalProvider.Equals("GitHub"))
                return typeof(GitHubUser);
            else 
                return typeof(MobileConnectUser);
        }

        private User GetInstanceOfExternalProvider(string externalProvider)
        {
            if (externalProvider.Equals("GoogleOpenIdConnect"))
                return new GoogleUser();
            else if (externalProvider.Equals("Twitter"))
                return new TwitterUser();
            else if (externalProvider.Equals("LinkedIn"))
                return new LinkedInUser();
            else if (externalProvider.Equals("GitHub"))
                return new GitHubUser();
            else
                return new MobileConnectUser();
        }

        private async Task SignInAsync(string activeDirectoryUser, string mobileConnectUser, string externalProvider)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            UserId userId = new UserId(mobileConnectUser, GetTypeOfExternalProvider(externalProvider));

            User user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                user = GetInstanceOfExternalProvider(externalProvider);
                user.ActiveDirectoryUserId = activeDirectoryUser;
                user.MobileConnectUserId = mobileConnectUser;

                await UserManager.CreateAsync(user);
            }
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
        }


        
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
     
    }
}