namespace Helpa.Entities.CustomEntities
{
    public class NotificationDTO
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
