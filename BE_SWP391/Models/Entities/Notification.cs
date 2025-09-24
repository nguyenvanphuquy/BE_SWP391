using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }

    public DateTime? SentAt { get; set; }

    public string? Status { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
