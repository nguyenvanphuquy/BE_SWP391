namespace BE_SWP391.Models.DTOs.Request
{
    public class VehicleRequest
    {
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int? Year { get; set; }
        public string? Type { get; set; }
        public int? RangeKm { get; set; }


    }
}
