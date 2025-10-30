using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Download
{
    public int DownloadId { get; set; }

    public int TransactionId { get; set; }

    public int PackageId { get; set; }

    public DateTime? DownloadDate { get; set; }

    public string? FileUrl { get; set; }

    public string? FileHash { get; set; }

    public string? Status { get; set; }

    public virtual DataPackage Package { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
