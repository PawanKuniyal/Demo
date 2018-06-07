namespace Helpa.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USR.Recievers")]
    public partial class Reciever
    {
        [Key]
        public int ReceiverId { get; set; }

        public int JobId { get; set; }

        [StringLength(50)]
        public string ReceiverName { get; set; }

        public int ReceiverGenderId { get; set; }

        public int RecieverAge { get; set; }

        [Required]
        [StringLength(1)]
        public string RowStatus { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual Job Job { get; set; }
    }
}
