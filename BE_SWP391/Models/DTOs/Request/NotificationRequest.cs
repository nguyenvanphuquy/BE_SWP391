namespace BE_SWP391.Models.DTOs.Request
{
    public class NotificationRequest
    {
        public string? Title { get; set; }

        public string? Message { get; set; }

        public DateTime? SentAt { get; set; }

        public string? Status { get; set; }

        public int UserId { get; set; }
    }
}
