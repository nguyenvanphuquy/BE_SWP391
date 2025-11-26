using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Status { get; set; }

    public decimal? Amount { get; set; }

    public string? Currency { get; set; }

    public int InvoiceId { get; set; }

    public int PlanId { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual PricingPlan Plan { get; set; } = null!;

    public virtual ICollection<RevenueShare> RevenueShares { get; set; } = new List<RevenueShare>();
}
