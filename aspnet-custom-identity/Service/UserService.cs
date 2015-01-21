using aspnet_custom_identity.Model;
using aspnet_custom_identity.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Service
{
   
    public class UserService : BaseService<User>,
                                IUserStore<User, UserId>,
                                IUserClaimStore<User, UserId>
    {

        public IUserRepository UserRepository { get; set; }

        
        public async Task CreateAsync(User user)
        {
            UserId userId = user.Id;
            User any = null;

            if (userId.Type.Name.Equals("TwitterUser"))
                any = await UserRepository.FindByTwitterUserId(userId.ExternalAuth);
            else if (userId.Type.Name.Equals("GoogleUser"))
                any = await UserRepository.FindByGoogleUserId(userId.ExternalAuth);
            else if (userId.Type.Name.Equals("LinkedInUser"))
                any = await UserRepository.FindByLinkedInUserId(userId.ExternalAuth);
            else if (userId.Type.Name.Equals("GitHubUser"))
                any = await UserRepository.FindByGitHubUserId(userId.ExternalAuth);
            else
                any = await UserRepository.FindByMobileConnectUserId(userId.ExternalAuth);
            
            if (any == null)
                user = UserRepository.Add(user);
            else
                user = UserRepository.ModifyActiveDirectoryName(any, user.ActiveDirectoryUserId);

            await UserRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            UserRepository.Delete(user);
            await UserRepository.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(UserId userId)
        {
            
            if (userId.Type.Name.Equals("TwitterUser"))
                return UserRepository.FindByTwitterUserId(userId.ExternalAuth);
            else if (userId.Type.Name.Equals("GoogleUser"))
                return UserRepository.FindByGoogleUserId(userId.ExternalAuth);
            else if (userId.Type.Name.Equals("LinkedInUser"))
                return UserRepository.FindByLinkedInUserId(userId.ExternalAuth);
            else if (userId.Type.Name.Equals("GitHubUser"))
                return UserRepository.FindByGitHubUserId(userId.ExternalAuth);
            else
                return UserRepository.FindByMobileConnectUserId(userId.ExternalAuth);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return UserRepository.FindByActiveDirectoryUserId(userName);
        }


        public async Task UpdateAsync(User user)
        {
            base.Update(user);
            await UserRepository.SaveChangesAsync();
        }

        public override void Dispose()
        {
            UserRepository.Dispose();
        }

        public Task AddClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            return Task.FromResult<object>(null);
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(User user)
        {
            IList<System.Security.Claims.Claim> claimList = new List<System.Security.Claims.Claim>();
            claimList.Add(new System.Security.Claims.Claim(ClaimTypes.Upn, user.ActiveDirectoryUserId.ToString()));
            claimList.Add(new System.Security.Claims.Claim(ClaimTypes.Email, string.Format("{0}@tid.es", user.ActiveDirectoryUserId)));
            claimList.Add(new System.Security.Claims.Claim("http://schemas.xmlsoap.org/claims/CommonName", user.ActiveDirectoryUserId));
            
            return Task.FromResult<IList<System.Security.Claims.Claim>>(claimList);
        }

        public Task RemoveClaimAsync(User user, System.Security.Claims.Claim claim)
        {
            return Task.FromResult<object>(null);
        }
    }
}
