namespace BE_SWP391.Models.DTOs.Response
{
    public class ReportTransactionResponse
    {
        public decimal TotalTransaction { get; set; }
        public decimal? TotalAmount { get; set; }
        public double AverageTransaction { get; set; }
        public double SuccessPercentage { get; set; }
        public double FailurePercentage { get; set; }
        public double PendingPercentage { get; set; }
    }
}
