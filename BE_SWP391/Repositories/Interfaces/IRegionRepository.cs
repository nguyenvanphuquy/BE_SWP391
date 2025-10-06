using BE_SWP391.Models;
using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IRegionRepository
    {
        Region? GetById(int id);
        IEnumerable<Region> GetAll();
        void Create(Region region);
        void Update(Region region);
        void Delete(Region region);
    }
}
