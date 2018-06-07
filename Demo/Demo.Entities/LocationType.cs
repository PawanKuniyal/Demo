namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCN.LocationTypes")]
    public partial class LocationType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocationType()
        {
            Helpers = new HashSet<Helper>();
        }

        public int LocationTypeId { get; set; }

        [Column("LocationType")]
        [Required]
        [StringLength(50)]
        public string LocationType1 { get; set; }

        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Helper> Helpers { get; set; }
    }
}
