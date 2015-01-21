using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Model
{
    public interface IAuditable
    {
        Auditable AuditInfo { get; set; }
    }

    public class Auditable
    {
        public Auditable()
        {
            CreationDate = ModificationDate = DateTime.Today;
            IsDeleted = false;
        }

        [Required]
        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }

        [Required]
        [Column("CreationDate")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Column("ModificationDate")]
        public DateTime ModificationDate { get; set; }
    }
}
