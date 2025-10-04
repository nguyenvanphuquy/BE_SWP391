﻿using System;
using System.Collections.Generic;

namespace BE_SWP391.Models.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
