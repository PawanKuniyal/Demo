namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StaticPage
    {
        public int StaticPageId { get; set; }

        [StringLength(255)]
        public string PageTitle { get; set; }

        [Column(TypeName = "text")]
        public string PageContent { get; set; }

        [Required]
        [StringLength(1)]
        public string PageType { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
