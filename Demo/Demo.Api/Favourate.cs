namespace Helpa.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NTW.Favourates")]
    public partial class Favourate
    {
        public int FavourateId { get; set; }

        public int UserId { get; set; }

        public int HelperId { get; set; }

        public bool Status { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual Helper Helper { get; set; }
    }
}
