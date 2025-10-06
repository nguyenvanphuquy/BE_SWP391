namespace BE_SWP391.Models.DTOs.Response
{
    public class VehicleResponse
    {
        public int VehicleId { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int? Year { get; set; }
        public string? Type { get; set; }
        public int? RangeKm { get; set; }
    }
}
