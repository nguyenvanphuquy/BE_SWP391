using BE_SWP391.Models.Entities;
using System;
using System.Collections.Generic;

namespace BE_SWP391.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public DateOnly? IssueDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Currency { get; set; }

    public decimal? TaxAmount { get; set; }

    public int UserId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
