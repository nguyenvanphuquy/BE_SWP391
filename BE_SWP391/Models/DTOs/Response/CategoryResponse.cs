namespace BE_SWP391.Models.DTOs.Response
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
