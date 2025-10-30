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

    public virtual ICollection<Download> Downloads { get; set; } = new List<Download>();

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual ICollection<PricingPlan> PricingPlans { get; set; } = new List<PricingPlan>();

    public virtual ICollection<RevenueShare> RevenueShares { get; set; } = new List<RevenueShare>();
}
