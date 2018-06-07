namespace Helpa.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LCN.HelperLocations")]
    public partial class HelperLocation
    {
        public int HelperLocationId { get; set; }

        public int HelperId { get; set; }

        public int LocationId { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual Helper Helper { get; set; }

        public virtual Location1 Location { get; set; }
    }
}
