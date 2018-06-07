namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USR.Helpers")]
    public partial class Helper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Helper()
        {
            Documents = new HashSet<Document>();
            Carousels = new HashSet<Carousel>();
            HelperLocations = new HashSet<HelperLocation>();
            Favourates = new HashSet<Favourate>();
            ReviewAndRatings = new HashSet<ReviewAndRating>();
            HelperServices = new HashSet<HelperService>();
        }

        public int HelperId { get; set; }

        public int UserId { get; set; }

        public int? LocationType { get; set; }

        [StringLength(50)]
        public string Qualification { get; set; }

        public int? Experience { get; set; }

        public int? MinAgeGroup { get; set; }

        public int? MaxAgeGroup { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool Status { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document> Documents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carousel> Carousels { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HelperLocation> HelperLocations { get; set; }

        public virtual LocationType LocationType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favourate> Favourates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewAndRating> ReviewAndRatings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HelperService> HelperServices { get; set; }
    }
}
