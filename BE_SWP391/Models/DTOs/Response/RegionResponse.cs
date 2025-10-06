namespace BE_SWP391.Models.DTOs.Response
{
    public class RegionResponse
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = null!;

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Description { get; set; }
    }
}
