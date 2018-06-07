namespace Helpa.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RTG.ReviewComments")]
    public partial class ReviewComment
    {
        public int ReviewCommentId { get; set; }

        public int ReviewId { get; set; }

        public int UserId { get; set; }

        [Column("ReviewComment")]
        [Required]
        [StringLength(500)]
        public string ReviewComment1 { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual ReviewAndRating ReviewAndRating { get; set; }
    }
}
