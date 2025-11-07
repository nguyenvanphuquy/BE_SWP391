namespace BE_SWP391.Models.DTOs.Response
{
    public class ListPricingResponse
    {
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public string status { get; set; }
    }
}
