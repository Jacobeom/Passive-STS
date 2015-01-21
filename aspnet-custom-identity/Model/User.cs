using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Model
{

    public class UserId : IEquatable<UserId>
    {
        public UserId(string externalAuth, System.Type type)
        {
            ExternalAuth = externalAuth;
            Type= type;
        }

        public string ExternalAuth { get; set; }
        public System.Type Type { get; set; }

        public bool Equals(UserId other)
        {
            return (this.ExternalAuth.Equals(other.ExternalAuth) && this.Type.Equals(other.Type));
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Type.Name, ExternalAuth);
        }
    }

    [Table("UserMappings")]
    public abstract class User : BaseModel, IUser<UserId>
    {
        [Required]
        [MaxLength(255)]
        public string MobileConnectUserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ActiveDirectoryUserId { get; set; }

        public abstract string UserType { get; }

        public UserId Id
        {
            get { return new UserId(MobileConnectUserId,this.GetType()); }
        }

        [NotMapped]
        public string UserName
        {
            get
            {
                return Id.ToString();
            }
            set
            {
                return;
            }
        }
    }
}
