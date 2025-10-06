using BE_SWP391.Models.Entities;
namespace BE_SWP391.Models.DTOs.Request
{
    public class BatteryRequest
    {
        public string? BatteryType { get; set; }

        public decimal? CapacityKWh { get; set; }

        public decimal? ChargingTime { get; set; }

        public int? CycleLife { get; set; }

        public string? Manufacturer { get; set; }

    }
}
