using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
namespace BE_SWP391.Services.Interfaces
{
    public interface IDataPackageService
    {
        DataPackageResponse? GetById(int id);
        IEnumerable<DataPackageResponse> GetAll();
        DataPackageResponse? Create(DataPackageRequest request);
        DataPackageResponse? Update(int id, DataPackageRequest request);
        bool Detele(int id);

        object GetStatusCount();
        List<DataForAdminResponse> GetDataForAdmin();
        bool ChageStatus(int PackedId, ChageStatusRequest request   );

    }
}
