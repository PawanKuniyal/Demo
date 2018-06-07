namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USR.JobScope")]
    public partial class JobScope
    {
        public int JobScopeId { get; set; }

        public int JobServiceId { get; set; }

        public int ScopeId { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Scope Scope { get; set; }

        public virtual JobService JobService { get; set; }
    }
}
