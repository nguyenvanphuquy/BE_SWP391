using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
namespace BE_SWP391.Services.Interfaces
{
    public interface IMetaDataService
    {
        MetaDataResponse? GetById(int id);
        IEnumerable<MetaDataResponse> GetAll();
        MetaDataResponse? Create(MetaDataRequest request);
        MetaDataResponse? Update(int id, MetaDataRequest request);
        bool Delete(int id);

    }
}
