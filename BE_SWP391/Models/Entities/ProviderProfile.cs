using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class ProviderProfile
{
    public int ProviderId { get; set; }

    public string? CompanyName { get; set; }

    public string? TaxId { get; set; }

    public string? BankAccount { get; set; }

    public decimal? ProviderRating { get; set; }

    public decimal? TotalRevenue { get; set; }

    public virtual User Provider { get; set; } = null!;
}
