using BE_SWP391.Data;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Implementations;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BE_SWP391.Services.Implementations
{
    public class DataPackageService : IDataPackageService
    {
        private readonly IDataPackageRepository _dataPackageRepository;
        private readonly EvMarketContext _context;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public DataPackageService(IDataPackageRepository dataPackageRepository, EvMarketContext context, ISubCategoryRepository subCategoryRepository)
        {
            _dataPackageRepository = dataPackageRepository;
            _context = context;
            _subCategoryRepository = subCategoryRepository;
        }
        public DataPackageResponse? GetById(int id)
        {
            var dataPackage = _dataPackageRepository.GetById(id);
            return dataPackage == null ? null : ToResponse(dataPackage);
        }
        public IEnumerable<DataPackageResponse> GetAll()
        {
            return _dataPackageRepository.GetAll().Select(ToResponse);
        }
        public CreatePackageResponse Create(DataPackageRequest request)
        {
            // 1️⃣ Tìm SubCategory theo tên
            var sub = _subCategoryRepository.GetByName(request.SubCategoryName);
            if (sub == null)
            {
                throw new Exception($"Không tìm thấy SubCategory với tên: {request.SubCategoryName}");
            }

            // 2️⃣ Lưu MetaData
            var meta = new MetaData
            {
                Type = request.MetaData.Type,
                Title = request.MetaData.Title,
                Description = request.MetaData.Description,
                Keywords = request.MetaData.Keywords,
                FileFormat = request.MetaData.FileFormat,
                FileSize = request.MetaData.FileSize,
                CreatedAt = DateTime.Now
            };
            _context.MetaDatas.Add(meta);
            _context.SaveChanges();

            // 3️⃣ Tạo DataPackage
            var data = new DataPackage
            {
                PackageName = request.PackageName,
                Description = request.Description,
                Version = request.Version,
                SubcategoryId = sub.SubcategoryId,
                UserId = request.UserId,
                MetadataId = meta.MetadataId,
                ReleaseDate = DateOnly.FromDateTime(DateTime.Now),
                LastUpdate = DateTime.Now,
                Status = "Pending"
            };
            _dataPackageRepository.Create(data);
            _context.SaveChanges();

            // 4️⃣ Trả về Response
            return new CreatePackageResponse
            {
                PackageId = data.PackageId,
                PackageName = data.PackageName,
                Description = data.Description ?? "",
                Version = data.Version ?? "",
                SubCategoryName = sub.SubcategoryName,
                UserId = data.UserId,

                Type = meta.Type,
                Title = meta.Title ?? "",
                MetaDescription = meta.Description ?? "",
                Keywords = meta.Keywords ?? "",
                FileFormat = meta.FileFormat ?? "",
                FileSize = meta.FileSize ?? 0,

                ReleaseDate = data.ReleaseDate,
                LastUpdate = data.LastUpdate,
                Status = data.Status,
                Message = "Tạo gói dữ liệu thành công!"
            };
        }

        public DataPackageResponse? Update(int id, DataPackageRequest request)
        {
            var dataPackage = _dataPackageRepository.GetById(id);
            if (dataPackage == null)
            {
                return null;
            }
            dataPackage.PackageName = request.PackageName;
            dataPackage.Description = request.Description;
            dataPackage.Version = request.Version;
            //dataPackage.ReleaseDate = request.ReleaseDate;
            dataPackage.LastUpdate = DateTime.Now;
            //dataPackage.Status = request.Status;
            dataPackage.UserId = request.UserId;
            //dataPackage.SubcategoryId = request.SubcategoryId;
            //dataPackage.MetadataId = request.MetadataId;
            _dataPackageRepository.Update(dataPackage);
            return ToResponse(dataPackage);
        }
        public bool Detele(int id)
        {
            var dataPackage = _dataPackageRepository.GetById(id);
            if (dataPackage == null)
            {
                return false;
            }
            _dataPackageRepository.Delete(dataPackage);
            return true;
        }
        public object GetStatusCount()
        {
            var pending = _dataPackageRepository.CountPending();
            var approved = _dataPackageRepository.CountApproved();
            var rejected = _dataPackageRepository.CountRejected();
            return new
            {
                pendingCount = pending,
                approvedCount = approved,
                rejectedCount = rejected
            };
        }
        public List<DataForAdminResponse> GetDataForAdmin()
        {
            return _dataPackageRepository.GetDataForAdmin();
        }
        public static DataPackageResponse ToResponse(DataPackage dataPackage)
        {
            return new DataPackageResponse
            {
                PackageId = dataPackage.PackageId,
                PackageName = dataPackage.PackageName,
                Description = dataPackage.Description,
                Version = dataPackage.Version,
                ReleaseDate = dataPackage.ReleaseDate,
                LastUpdate = dataPackage.LastUpdate,
                Status = dataPackage.Status,
                UserId = dataPackage.UserId,
                SubcategoryId = dataPackage.SubcategoryId,
                MetadataId = dataPackage.MetadataId
            };
        }
        public bool ChageStatus(int DataPackageId, ChageStatusRequest request)
        {
            var dataPackage = _dataPackageRepository.GetById(DataPackageId);
            if (dataPackage == null)
            {
                throw new Exception("Không tìm thấy dữ liệu");
            }
            var validStatus = new[] { "Pending", "Approved", "Rejected" };
            if (!validStatus.Contains(request.newStatus))
            {
                throw new Exception("Trạng thái không hợp lệ");
            }
            dataPackage.Status = request.newStatus;
            dataPackage.LastUpdate = DateTime.UtcNow;
            _dataPackageRepository.ChageStatus(dataPackage);
            return true;

        }
        public List<DataPendingRepsonse> GetPendingData()
        {
            return _dataPackageRepository.GetDataPending();
        }
        public List<AllPackageResponse> GetAllPackages()
        {
            return _dataPackageRepository.GetAllPackage();
        }
        public UserDataStatsResponse GetUserDataStats(int userId)
        {
            return _dataPackageRepository.GetUserDataStats(userId);
        }
        public List<UserDataResponse> GetUserDataByUserId(int userId)
        {
            return _dataPackageRepository.GetUserDataByUserId(userId);
        }
        public List<DataForUserResponse> GetDataForUser(int userId)
        {
            return _dataPackageRepository.GetDataForUser(userId);
        }
        public ReportOrderResponse GetReportOrder(int userId)
        {
            return _dataPackageRepository.GetReportOrder(userId);
        }
        public PackageDetailResponse GetPackageDetails(int packageId)
        {
            return _dataPackageRepository.GetPackageDetails(packageId);
        }
    }
}
