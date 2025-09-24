using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class VehicleMetaData
{
    public int Id { get; set; }

    public int VehicleId { get; set; }

    public int MetadataId { get; set; }

    public virtual MetaData Metadata { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
