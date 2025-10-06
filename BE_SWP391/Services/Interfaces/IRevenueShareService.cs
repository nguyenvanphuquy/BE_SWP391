using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
namespace BE_SWP391.Services.Interfaces
{
    public interface IRevenueShareService
    {
        RevenueShareResponse? GetById(int id);
        IEnumerable<RevenueShareResponse> GetAll();
        RevenueShareResponse? Create(RevenueShareRequest request);
        RevenueShareResponse? Update(int id, RevenueShareRequest request);
        bool Delete(int id);
    }
}
