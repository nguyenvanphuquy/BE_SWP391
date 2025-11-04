namespace BE_SWP391.Models.DTOs.Request
{
    public class UpdateCartQuantityRequest
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
    }
}
