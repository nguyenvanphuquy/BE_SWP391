using BE_SWP391.Data;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EvMarketContext _marketContext;
        public RoleRepository(EvMarketContext marketContext)
        {
            _marketContext = marketContext;
        }
        public Role? GetById(int id)
        {
            return _marketContext.Roles.FirstOrDefault(r => r.RoleId == id);
        }
        public Role? GetByName(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return null;

            string normalizedName = roleName.Trim().ToLower();
            return _marketContext.Roles
                .FirstOrDefault(r => r.RoleName.ToLower() == normalizedName);
        }
    }
}
