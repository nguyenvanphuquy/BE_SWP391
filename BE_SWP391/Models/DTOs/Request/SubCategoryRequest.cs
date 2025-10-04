namespace BE_SWP391.Models.DTOs.Request
{
    public class SubCategoryRequest
    {
        public string SubcategoryName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? Description { get; set; }
    }
}
