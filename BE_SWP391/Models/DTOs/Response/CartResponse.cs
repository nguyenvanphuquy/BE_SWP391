namespace BE_SWP391.Models.DTOs.Response
{
    public class CartResponse
    {
        public int CartId { get; set; }
        public int PlanId { get; set; }
        public string PackageName { get; set; }
        public string ProviderName { get; set; }
        public int? Quantity { get; set; }
        public string Type { get; set; }
        public string? FileFormat { get; set; }
        public decimal? TotalAmout { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
