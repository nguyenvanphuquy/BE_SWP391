namespace BE_SWP391.Models.DTOs.Response
{
    public class AddToCartResponse
    {
        public int UserId { get; set; }
        public int? TotalItems { get; set; }
        public string Message { get; set; }
    }
}