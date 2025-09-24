using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int? Year { get; set; }

    public string? Type { get; set; }

    public int? RangeKm { get; set; }

    public virtual ICollection<VehicleMetaData> VehicleMetaData { get; set; } = new List<VehicleMetaData>();
}
