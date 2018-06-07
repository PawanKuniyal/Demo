namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USR.Jobs")]
    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            JobServices = new HashSet<JobService>();
            Recievers = new HashSet<Reciever>();
        }

        public int JobId { get; set; }

        public int CreatedUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string JobTiltle { get; set; }

        [Required]
        public string JobDescription { get; set; }

        [Required]
        [StringLength(1)]
        public string JobType { get; set; }

        [Required]
        [StringLength(1)]
        public string HelperType { get; set; }

        public int LocationId { get; set; }

        public int TimeId { get; set; }

        public int PriceId { get; set; }

        public bool Status { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual JobLocation JobLocation { get; set; }

        public virtual JobPrice JobPrice { get; set; }

        public virtual JobTime JobTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobService> JobServices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reciever> Recievers { get; set; }
    }
}
