namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SVC.Scopes")]
    public partial class Scope
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Scope()
        {
            HelperServiceScopes = new HashSet<HelperServiceScope>();
            JobScopes = new HashSet<JobScope>();
        }

        public int ScopeId { get; set; }

        public int ServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string ScopeName { get; set; }

        public bool Status { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HelperServiceScope> HelperServiceScopes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobScope> JobScopes { get; set; }

        public virtual Service Service { get; set; }
    }
}
