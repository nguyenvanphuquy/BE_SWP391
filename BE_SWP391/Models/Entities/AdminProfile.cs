using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class AdminProfile
{
    public int AdminId { get; set; }

    public string? Permissions { get; set; }

    public virtual User Admin { get; set; } = null!;
}
