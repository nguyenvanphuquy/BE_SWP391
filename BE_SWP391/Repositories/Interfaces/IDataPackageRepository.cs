using BE_SWP391.Data;
using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IDataPackageRepository
    {
        DataPackage? GetById(int id);
        IEnumerable<DataPackage> GetAll();
        void Create(DataPackage dataPackage);
        void Update(DataPackage dataPackage);
        void Delete(DataPackage dataPackage);
        int CountPending();
        int CountApproved();
        int CountRejected();
    }
}
