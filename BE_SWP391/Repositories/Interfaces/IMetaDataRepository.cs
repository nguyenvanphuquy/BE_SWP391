using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IMetaDataRepository
    {
        MetaData? GetById(int id);
        IEnumerable<MetaData> GetAll();
        void Create(MetaData metadata);
        void Update(MetaData metadata);
        void Delete(int id);

    }
}
