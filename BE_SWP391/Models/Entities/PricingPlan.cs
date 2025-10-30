using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class PricingPlan
{
    public int PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Currency { get; set; }

    public int? Duration { get; set; }

    public string? AccessType { get; set; }

    public int PackageId { get; set; }

    public decimal? Discount { get; set; }

    public int TransactionId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual DataPackage Package { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
