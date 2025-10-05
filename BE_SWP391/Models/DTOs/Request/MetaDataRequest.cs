namespace BE_SWP391.Models.DTOs.Request
{
    public class MetaDataRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public string? FileFormat { get; set; }
        public long? FileSize { get; set; }
    }
}
