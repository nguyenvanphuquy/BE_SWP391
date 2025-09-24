using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class SubCategory
{
    public int SubcategoryId { get; set; }

    public string SubcategoryName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Datapackage> Datapackages { get; set; } = new List<Datapackage>();
}
