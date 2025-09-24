using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? Rating { get; set; }

    public string? Title { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int UserId { get; set; }

    public int PackageId { get; set; }

    public virtual Datapackage Package { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
