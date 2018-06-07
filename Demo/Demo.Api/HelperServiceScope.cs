namespace Helpa.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SVC.HelperServiceScope")]
    public partial class HelperServiceScope
    {
        public int HelperServiceScopeId { get; set; }

        public int HelperServiceId { get; set; }

        public int HelperScopeId { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual HelperService HelperService { get; set; }

        public virtual Scope Scope { get; set; }
    }
}
