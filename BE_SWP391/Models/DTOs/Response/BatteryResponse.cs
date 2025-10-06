namespace BE_SWP391.Models.DTOs.Response
{
    public class BatteryResponse
    {
        public int BatteryId { get; set; }

        public string? BatteryType { get; set; }

        public decimal? CapacityKWh { get; set; }

        public decimal? ChargingTime { get; set; }

        public int? CycleLife { get; set; }

        public string? Manufacturer { get; set; }
    }
}
