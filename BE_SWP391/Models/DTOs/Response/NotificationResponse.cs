namespace BE_SWP391.Models.DTOs.Response
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }
        public string? Title { get; set; }

        public string? Message { get; set; }

        public DateTime? SentAt { get; set; }

        public string? Status { get; set; }

        public int UserId { get; set; }
    }
}
