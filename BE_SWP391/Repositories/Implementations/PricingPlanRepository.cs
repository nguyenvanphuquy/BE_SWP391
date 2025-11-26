using BE_SWP391.Data;
using BE_SWP391.Models;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
namespace BE_SWP391.Repositories.Implementations
{
    public class PricingPlanRepository : IPricingPlanRepository
    {
        private readonly EvMarketContext _context;
        public PricingPlanRepository(EvMarketContext context)
        {
            _context = context;
        }
        public PricingPlan? GetById(int id)
        {
            return _context.PricingPlans.Find(id);
        }
        public IEnumerable<PricingPlan> GetAll()
        {
            return _context.PricingPlans.ToList();
        }
        public PricingPlanResponse Create(PricingPlanRequest request)
        {
            if (request.Price <= 0)
            {
                throw new Exception("Giá (Price) phải lớn hơn 0.");
            }
            if (request.Duration <= 0)
            {
                throw new Exception("Thời gian sử dụng (Duration) phải lớn hơn 0.");
            }
            if (request.Discount < 0)
            {
                throw new Exception("Giảm giá (Discount) không được nhỏ hơn 0.");
            }

            var package = _context.DataPackages.FirstOrDefault(dp => dp.PackageName == request.PackageName);
            if (package == null)
            {
                throw new Exception($"Không tìm thấy gói dữ liệu với tên: {request.PackageName}");
            }
            var existingPlan = _context.PricingPlans.FirstOrDefault(p => p.PackageId == package.PackageId);
            if (existingPlan != null)
            {
                throw new Exception($"Gói dữ liệu '{request.PackageName}' đã có gói giá. Vui lòng chỉnh sửa thay vì tạo mới.");
            }
            var pricingPlan = new PricingPlan
            {
                PlanName = request.PlanName,
                Price = request.Price,
                Currency = request.Currency,
                Duration = request.Duration,
                AccessType = request.AccessType,
                PackageId = package.PackageId,
                Discount = request.Discount
            };
            _context.PricingPlans.Add(pricingPlan);
            _context.SaveChanges();
            return new PricingPlanResponse
            {
                PlanId = pricingPlan.PlanId,
                PlanName = pricingPlan.PlanName,
                Price = pricingPlan.Price,
                Currency = pricingPlan.Currency,
                Duration = pricingPlan.Duration,
                AccessType = pricingPlan.AccessType,
                PackageId = pricingPlan.PackageId,
                Discount = pricingPlan.Discount
            };
        }
        public UpdatePricingResponse UpdatePricing(UpdatePricingRequest request)
        {
            var pricingPlan = _context.PricingPlans.Find(request.PricingPlanId);
            if (pricingPlan == null)
            {
                throw new Exception("Pricing plan not found");
            }
            var oldPrice = pricingPlan.Price;
            pricingPlan.Price = request.NewPrice;
            _context.SaveChanges();
            var dataPackage = _context.DataPackages.Find(pricingPlan.PackageId);
            return new UpdatePricingResponse
            {
                PricingPlanId = pricingPlan.PlanId,
                PacageName = dataPackage != null ? dataPackage.PackageName : "Unknown",
                Description = dataPackage != null ? dataPackage.Description : "No description",
                OldPrice = oldPrice,
                NewPrice = pricingPlan.Price
            };
        }
        public void Delete(PricingPlan pricingPlan)
        {
                _context.PricingPlans.Remove(pricingPlan);
                _context.SaveChanges();
        }
        public ReportPricingStaffResponse GetReportPricingStaff(int userId)
        {
            var packages = (from dp in _context.DataPackages
                            join pp in _context.PricingPlans on dp.PackageId equals pp.PackageId
                            where dp.UserId == userId
                            select pp).ToList();

            var packageCount = packages.Count();
            if (packageCount == 0)
            {
                return new ReportPricingStaffResponse
                {
                    AvenragePricing = 0,
                    PackageCount = 0,
                    PricingPlan = 0
                };
            }
            var totalPricing = packages.Sum(pp => pp.Price);
            var averagePricing = totalPricing / packageCount;
            return new ReportPricingStaffResponse
            {
                AvenragePricing = averagePricing,
                PackageCount = packageCount,
                PricingPlan = packageCount
            };
        }
        public List<ListPricingResponse> GetListPricing(int userId)
        {
            var query = (from pp in _context.PricingPlans
                         join dp in _context.DataPackages on pp.PackageId equals dp.PackageId into dpGroup
                         from dp in dpGroup.DefaultIfEmpty()
                         where dp.UserId == userId
                         orderby dp.ReleaseDate descending
                         select new ListPricingResponse
                        {
                            PricingId = pp.PlanId,
                             PackageName = dp.PackageName,
                            Description = dp.Description,
                            Price = pp.Price,
                            status = dp.Status

                        } )
                        .ToList();
            return query;
        }
        public ReportRevenueResponse GetRevenueReport(int userId)
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfPrevMonth = startOfMonth.AddMonths(-1);
            var endOfPrevMonth = startOfMonth.AddDays(-1);

            var currentRevenue = _context.Transactions
                .Where(t => t.Status == "completed" && t.TransactionDate >= startOfMonth && t.Invoice.UserId == userId).Sum(t => t.Amount);
            var prevRevenue = _context.Transactions
                            .Where(t => t.Status == "completed"
                                     && t.TransactionDate >= startOfPrevMonth
                                     && t.TransactionDate <= endOfPrevMonth
                                     && t.Invoice.UserId == userId)
                            .Sum(t => t.Amount) ?? 0;
            var revenueGrowth = prevRevenue == 0 ? 100 : ((currentRevenue - prevRevenue) / prevRevenue) * 100;

            var currentDownloads = _context.Downloads
               .Count(d => d.Package.UserId == userId && d.DownloadDate >= startOfMonth);

            var prevDownloads = _context.Downloads
                .Count(d => d.Package.UserId == userId && d.DownloadDate >= startOfPrevMonth && d.DownloadDate <= endOfPrevMonth);

            var downloadGrowth = prevDownloads == 0 ? 100 : ((currentDownloads - prevDownloads) / prevDownloads) * 100;


            var totalBuyers = _context.Transactions
                .Where(t => t.Status == "completed"
                         && t.Invoice.UserId == userId)
                .Select(t => t.Invoice.UserId)
                .Distinct()
                .Count();

            var newBuyers = _context.Transactions
                .Where(t => t.Status == "completed"
                         && t.TransactionDate >= startOfMonth
                         && t.Invoice.UserId == userId)
                .Select(t => t.Invoice.UserId)
                .Distinct()
                .Count();


            var totalTransactions = _context.Transactions
                .Count(t => t.Status == "completed"
                         && t.TransactionDate >= startOfMonth
                         && t.Invoice.UserId == userId);

            var avgTransactionValue = totalTransactions == 0 ? 0 : currentRevenue / totalTransactions;

            return new ReportRevenueResponse
            {
                TotalRevenue = currentRevenue,
                RevenueGrowth = revenueGrowth,
                DownloadCount = currentDownloads,
                DownloadGrowth = downloadGrowth,
                BuyerCount = totalBuyers,
                NewBuyer = newBuyers,
                AverageRevenue = avgTransactionValue
            };
        }
    }
}
