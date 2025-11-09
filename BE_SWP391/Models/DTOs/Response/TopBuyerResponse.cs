namespace BE_SWP391.Models.DTOs.Response
{
    public class TopBuyerResponse
    {
        public string BuyerName { get; set; }
        public int TransactionCount { get; set; }
        public decimal? TotalRevenue { get; set; }
    }
}
