namespace BE_SWP391.Models.DTOs.Request
{
    public class AddToCartRequest
    {
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}