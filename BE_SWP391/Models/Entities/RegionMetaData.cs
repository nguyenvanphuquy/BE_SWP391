using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class RegionMetaData
{
    public int Id { get; set; }

    public int RegionId { get; set; }

    public int MetadataId { get; set; }

    public virtual MetaData Metadata { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
