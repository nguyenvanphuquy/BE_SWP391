using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface IRevenueShareRepository
    {
        RevenueShare? GetById(int id);
        IEnumerable<RevenueShare> GetAll();
        void Create(RevenueShare revenueShare);
        void Update(RevenueShare revenueShare);
        void Delete(RevenueShare revenueShare);
        List<ProfitResponse> GetProfit();


    }
}
