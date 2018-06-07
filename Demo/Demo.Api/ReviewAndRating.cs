namespace Helpa.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RTG.ReviewAndRating")]
    public partial class ReviewAndRating
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReviewAndRating()
        {
            ReviewComments = new HashSet<ReviewComment>();
        }

        [Key]
        public int ReviewId { get; set; }

        public int UserId { get; set; }

        public int HelperId { get; set; }

        public int Rating { get; set; }

        [StringLength(500)]
        public string Review { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual Helper Helper { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewComment> ReviewComments { get; set; }
    }
}
