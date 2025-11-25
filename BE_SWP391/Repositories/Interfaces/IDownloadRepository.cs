using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IDownloadRepository
    {
        Download GetById(int id);
        IEnumerable<Download> GetAll();
        IEnumerable<Download> GetByPackageId(int packageId);
        void Create(Download download);
        void Update(Download download);
        void Delete(Download download);

    }
}
