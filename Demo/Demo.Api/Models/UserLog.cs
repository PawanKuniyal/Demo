using System;
using System.ComponentModel.DataAnnotations;

namespace Helpa.Api.Models
{
    public class UserLog
    {
        [Key]
        public int UserLogId { get; set; }

        public int UserId { get; set; }

        public string DeviceId { get; set; }

        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}