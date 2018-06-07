namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SVC.HelperServices")]
    public partial class HelperService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HelperService()
        {
            HelperServiceScopes = new HashSet<HelperServiceScope>();
        }

        public int HelperServiceId { get; set; }

        public int ServiceId { get; set; }

        public int HelperId { get; set; }

        public bool HourPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MinHourPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MaxHourPrice { get; set; }

        public bool DayPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MinDayPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MaxDayPrice { get; set; }

        public bool MonthlyPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MinMonthPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MaxMonthPrice { get; set; }

        public bool Status { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual Helper Helper { get; set; }

        public virtual Service Service { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HelperServiceScope> HelperServiceScopes { get; set; }
    }
}
