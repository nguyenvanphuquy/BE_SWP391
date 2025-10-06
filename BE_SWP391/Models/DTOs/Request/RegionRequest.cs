namespace BE_SWP391.Models.DTOs.Request
{
    public class RegionRequest
    {
        public string RegionName { get; set; } = null!;

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Description { get; set; }
    }
}
