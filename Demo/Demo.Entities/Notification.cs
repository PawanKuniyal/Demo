namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Notification
    {
        public int NotificationId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        [StringLength(50)]
        public string Url { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        [StringLength(1)]
        public string RowStatus { get; set; }

        [StringLength(1)]
        public string NotificationState { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ReadDate { get; set; }

        [StringLength(1)]
        public string NotificationType { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual AspNetUser AspNetUser1 { get; set; }
    }
}
