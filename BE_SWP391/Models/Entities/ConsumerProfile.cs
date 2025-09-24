using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class ConsumerProfile
{
    public int ConsumerId { get; set; }

    public string? SubscriptionLevel { get; set; }

    public int? DownloadLimit { get; set; }

    public string? BillingAddress { get; set; }

    public virtual User Consumer { get; set; } = null!;
}
