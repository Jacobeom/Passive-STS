using aspnet_custom_identity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindByActiveDirectoryUserId(string activeDirectoryUserId);

        User ModifyActiveDirectoryName(User entity,string activeDirectoryUserId);


        Task<User> FindByMobileConnectUserId(string mobileConnectUserId);
        Task<User> FindByTwitterUserId(string twitterUserId);
        Task<User> FindByLinkedInUserId(string linkedInUserId);
        Task<User> FindByGoogleUserId(string googleUserId);
        Task<User> FindByGitHubUserId(string gitHubUserId);
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        protected IDbSet<User> UserDbSet
        {
            get { return (IDbSet<User>)this.DbSet; }
        }

        public Task<User> FindByMobileConnectUserId(string mobileConnectUserId)
        {
            return UserDbSet.FirstOrDefaultAsync(x => (x.MobileConnectUserId.Equals(mobileConnectUserId) &&
                                                                     x.AuditInfo.IsDeleted.Equals(false))&&
                                                                     x is MobileConnectUser);
        }

        public Task<User> FindByTwitterUserId(string twitterUserId)
        {
            return UserDbSet.FirstOrDefaultAsync(x => (x.MobileConnectUserId.Equals(twitterUserId) &&
                                                                     x.AuditInfo.IsDeleted.Equals(false)) &&
                                                                     x is TwitterUser);
        }

        public Task<User> FindByLinkedInUserId(string linkedInUserId)
        {
            return UserDbSet.FirstOrDefaultAsync(x => (x.MobileConnectUserId.Equals(linkedInUserId) &&
                                                                     x.AuditInfo.IsDeleted.Equals(false)) &&
                                                                     x is LinkedInUser);
        }

        public Task<User> FindByGoogleUserId(string googleUserId)
        {
            return UserDbSet.FirstOrDefaultAsync(x => (x.MobileConnectUserId.Equals(googleUserId) &&
                                                                     x.AuditInfo.IsDeleted.Equals(false)) &&
                                                                     x is GoogleUser);
        }

        public Task<User> FindByGitHubUserId(string gitHubUserId)
        {
            return UserDbSet.FirstOrDefaultAsync(x => (x.MobileConnectUserId.Equals(gitHubUserId) &&
                                                                     x.AuditInfo.IsDeleted.Equals(false)) &&
                                                                     x is GitHubUser);
        }


        public Task<User> FindByActiveDirectoryUserId(string activeDirectoryUserId)
        {
             return UserDbSet.FirstOrDefaultAsync(x => (x.ActiveDirectoryUserId.Equals(activeDirectoryUserId) &&
                                                                     x.AuditInfo.IsDeleted.Equals(false)));   
        }

        public User ModifyActiveDirectoryName(User entity, string activeDirectoryUserId)
        {
            entity.ActiveDirectoryUserId = activeDirectoryUserId;
            ForceEdit(entity);
            return entity;
        }
    }
}
