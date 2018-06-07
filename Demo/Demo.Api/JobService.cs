namespace Helpa.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USR.JobServices")]
    public partial class JobService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobService()
        {
            JobScopes = new HashSet<JobScope>();
        }

        public int JobServiceId { get; set; }

        public int JobId { get; set; }

        public int ServiceId { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Service Service { get; set; }

        public virtual Job Job { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobScope> JobScopes { get; set; }
    }
}
