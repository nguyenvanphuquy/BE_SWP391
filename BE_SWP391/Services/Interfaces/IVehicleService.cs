using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Services.Interfaces
{
    public interface IVehicleService
    {
        VehicleResponse? GetById(int id);
        IEnumerable<VehicleResponse> GetAll();
        VehicleResponse? Create(VehicleRequest request);
        VehicleResponse? Update(int id, VehicleRequest request);
        bool Delete(int id);

    }
}
