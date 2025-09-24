using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Datapackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Version { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? Status { get; set; }

    public int UserId { get; set; }

    public int? SubcategoryId { get; set; }

    public int? MetadataId { get; set; }

    public virtual ICollection<Download> Downloads { get; set; } = new List<Download>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual MetaData? Metadata { get; set; }

    public virtual ICollection<PricingPlan> PricingPlans { get; set; } = new List<PricingPlan>();

    public virtual SubCategory? Subcategory { get; set; }

    public virtual User User { get; set; } = null!;
}
