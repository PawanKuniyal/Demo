namespace Helpa.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCN.Location")]
    public partial class Location1
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location1()
        {
            HelperLocations = new HashSet<HelperLocation>();
        }

        [Key]
        public int LocationId { get; set; }

        [StringLength(50)]
        public string Locality { get; set; }

        [StringLength(50)]
        public string District { get; set; }

        [Required]
        [StringLength(50)]
        public string LocationName { get; set; }

        [Required]
        public DbGeography LocationGeography { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HelperLocation> HelperLocations { get; set; }
    }
}
