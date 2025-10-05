using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? FullName { get; set; }

    public string? Organization { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int RoleId { get; set; }

    public virtual AdminProfile? AdminProfile { get; set; }

    public virtual ConsumerProfile? ConsumerProfile { get; set; }

    public virtual ICollection<DataPackage> Datapackages { get; set; } = new List<DataPackage>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ProviderProfile? ProviderProfile { get; set; }

    public virtual ICollection<RevenueShare> RevenueShares { get; set; } = new List<RevenueShare>();

    public virtual Role Role { get; set; } = null!;
}
