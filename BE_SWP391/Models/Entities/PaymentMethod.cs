using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class PaymentMethod
{
    public int MethodId { get; set; }

    public string? MethodName { get; set; }

    public string? Provider { get; set; }

    public string? Details { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int TransactionId { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
