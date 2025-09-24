using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class RevenueShare
{
    public int RevenueId { get; set; }

    public int ProviderId { get; set; }

    public decimal SharePercentage { get; set; }

    public decimal Amount { get; set; }

    public DateTime? DistributedAt { get; set; }

    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
