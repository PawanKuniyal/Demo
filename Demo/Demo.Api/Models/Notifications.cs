namespace Helpa.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Notifications
    {
        [Key]
        public int NotificationId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        [StringLength(1)]
        public string NotificationType { get; set; }

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

        public virtual ApplicationUser Sender { get; set; }

        public virtual ApplicationUser Receiver { get; set; }
    }
}