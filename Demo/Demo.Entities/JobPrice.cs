namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USR.JobPrice")]
    public partial class JobPrice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobPrice()
        {
            Jobs = new HashSet<Job>();
        }

        public int JobPriceId { get; set; }

        public bool Hourly { get; set; }

        public bool Daily { get; set; }

        public bool Monthly { get; set; }

        [Column(TypeName = "money")]
        public decimal? MinPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MaxPrice { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
