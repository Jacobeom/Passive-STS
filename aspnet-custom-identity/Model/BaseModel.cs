using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Model
{
    public abstract class BaseModel : IAuditable
    {
        public BaseModel()
        {
            AuditInfo = new Auditable();
        }

        [Key]
        public int? PrimaryId { get; set; }

        public Auditable AuditInfo { get; set; }
    }
}
