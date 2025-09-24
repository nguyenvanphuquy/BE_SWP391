using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Region
{
    public int RegionId { get; set; }

    public string RegionName { get; set; } = null!;

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<RegionMetaData> RegionMetaData { get; set; } = new List<RegionMetaData>();
}
