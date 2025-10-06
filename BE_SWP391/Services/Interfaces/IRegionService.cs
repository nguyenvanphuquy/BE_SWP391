using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Services.Interfaces
{
    public interface IRegionService
    {
        RegionResponse? GetById(int id);
        IEnumerable<RegionResponse> GetAll();
        RegionResponse? Create(RegionRequest request);
        RegionResponse? Update(int id, RegionRequest request);
        bool Delete(int id);
    }
}
