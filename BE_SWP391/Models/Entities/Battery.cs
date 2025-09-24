using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Battery
{
    public int BatteryId { get; set; }

    public string? BatteryType { get; set; }

    public decimal? CapacityKWh { get; set; }

    public decimal? ChargingTime { get; set; }

    public int? CycleLife { get; set; }

    public string? Manufacturer { get; set; }

    public virtual ICollection<BatteryMetaData> BatteryMetaData { get; set; } = new List<BatteryMetaData>();
}
