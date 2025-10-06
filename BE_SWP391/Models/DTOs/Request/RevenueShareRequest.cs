namespace BE_SWP391.Models.DTOs.Request
{
    public class RevenueShareRequest
    {
        public int ProviderId { get; set; }

        public decimal SharePercentage { get; set; }

        public decimal Amount { get; set; }

        public DateTime? DistributedAt { get; set; }

        public int TransactionId { get; set; }

        public int UserId { get; set; }
    }
}
