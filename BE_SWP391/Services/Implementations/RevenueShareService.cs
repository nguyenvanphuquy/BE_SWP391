using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Services.Implementations
{
    public class RevenueShareService : IRevenueShareService
    {
        private readonly IRevenueShareRepository _revenueShareRepository;
        public RevenueShareService(IRevenueShareRepository revenueShareRepository)
        {
            _revenueShareRepository = revenueShareRepository;
        }
        public RevenueShareResponse? GetById(int id)
        {
            var revenueShare = _revenueShareRepository.GetById(id);
            return revenueShare == null ? null : ToResponse(revenueShare);

        }
        public IEnumerable<RevenueShareResponse> GetAll()
        {
            return _revenueShareRepository.GetAll().Select(ToResponse);
        }
        public RevenueShareResponse? Create(RevenueShareRequest request)
        {
            var revenueShare = new RevenueShare
            {
                ProviderId = request.ProviderId,
                SharePercentage = request.SharePercentage,
                Amount = request.Amount,
                DistributedAt = request.DistributedAt,
                TransactionId = request.TransactionId,
                UserId = request.UserId
            };
            _revenueShareRepository.Create(revenueShare);
            return ToResponse(revenueShare);
        }
        public RevenueShareResponse? Update(int id, RevenueShareRequest request)
        {
            var revenueShare = _revenueShareRepository.GetById(id);
            if (revenueShare == null)
            {
                return null;
            }
            revenueShare.ProviderId = request.ProviderId;
            revenueShare.SharePercentage = request.SharePercentage;
            revenueShare.Amount = request.Amount;
            revenueShare.DistributedAt = request.DistributedAt;
            revenueShare.TransactionId = request.TransactionId;
            revenueShare.UserId = request.UserId;
            _revenueShareRepository.Update(revenueShare);
            return ToResponse(revenueShare);
        }
        public bool Delete(int id)
        {
            var revenueShare = _revenueShareRepository.GetById(id);
            if (revenueShare == null)
            {
                return false;
            }
            _revenueShareRepository.Delete(revenueShare);
            return true;
        }
        public static RevenueShareResponse ToResponse(RevenueShare revenueShare)
        {
            return new RevenueShareResponse
            {
                RevenueId = revenueShare.RevenueId,
                ProviderId = revenueShare.ProviderId,
                SharePercentage = revenueShare.SharePercentage,
                Amount = revenueShare.Amount,
                DistributedAt = revenueShare.DistributedAt,
                TransactionId = revenueShare.TransactionId,
                UserId = revenueShare.UserId
            };
        }
        public List<ProfitResponse> GetAllProfit()

        {
            return _revenueShareRepository.GetProfit();
        }

    }
}
