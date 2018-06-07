using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;

namespace Helpa.Api.Models
{
    public class Locations
    {
        [Key]
        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public DbGeography LocationGeography { get; set; }

        public bool RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<ApplicationUser> User { get; set; }
    }
}