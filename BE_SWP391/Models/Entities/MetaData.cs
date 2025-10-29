using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class MetaData
{
    public int MetadataId { get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Keywords { get; set; }

    public string? FileFormat { get; set; }

    public long? FileSize { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BatteryMetaData> BatteryMetaData { get; set; } = new List<BatteryMetaData>();

    public virtual ICollection<DataPackage> DataPackages { get; set; } = new List<DataPackage>();

    public virtual ICollection<RegionMetaData> RegionMetaData { get; set; } = new List<RegionMetaData>();

    public virtual ICollection<VehicleMetaData> VehicleMetaData { get; set; } = new List<VehicleMetaData>();
}
