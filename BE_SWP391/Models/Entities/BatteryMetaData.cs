using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class BatteryMetaData
{
    public int Id { get; set; }

    public int BatteryId { get; set; }

    public int MetadataId { get; set; }

    public virtual Battery Battery { get; set; } = null!;

    public virtual MetaData Metadata { get; set; } = null!;
}
