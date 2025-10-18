﻿using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Role? GetById(int id);
        Role? GetByName(string roleName);

    }
}
