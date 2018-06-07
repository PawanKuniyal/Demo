namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOC.Documents")]
    public partial class Document
    {
        public int DocumentId { get; set; }

        public int FileId { get; set; }

        public int HelperId { get; set; }

        [Required]
        [StringLength(1)]
        public string DocumentType { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual File File { get; set; }

        public virtual Helper Helper { get; set; }
    }
}
