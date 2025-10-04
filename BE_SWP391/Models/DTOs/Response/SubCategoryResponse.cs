namespace BE_SWP391.Models.DTOs.Response
{
    public class SubCategoryResponse
    {
        public int SubcategoryId { get; set; }
        public string SubcategoryName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
