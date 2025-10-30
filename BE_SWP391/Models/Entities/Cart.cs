using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public virtual PricingPlan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
