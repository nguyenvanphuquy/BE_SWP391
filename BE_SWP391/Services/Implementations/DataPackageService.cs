using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Implementations;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
namespace BE_SWP391.Services.Implementations
{
    public class DataPackageService : IDataPackageService
    {
        private readonly IDataPackageRepository _dataPackageRepository;

        public DataPackageService(IDataPackageRepository dataPackageRepository)
        {
            _dataPackageRepository = dataPackageRepository;
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
        public DataPackageResponse? Create(DataPackageRequest request)
        {
            var dataPackage = new DataPackage
            {
                PackageName = request.PackageName,
                Description = request.Description,
                Version = request.Version,
                ReleaseDate = request.ReleaseDate,
                LastUpdate = DateTime.Now,
                Status = request.Status,
                UserId = request.UserId,
                SubcategoryId = request.SubcategoryId,
                MetadataId = request.MetadataId
            };
            _dataPackageRepository.Create(dataPackage);
            return ToResponse(dataPackage);
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
            dataPackage.ReleaseDate = request.ReleaseDate;
            dataPackage.LastUpdate = DateTime.Now;
            dataPackage.Status = request.Status;
            dataPackage.UserId = request.UserId;
            dataPackage.SubcategoryId = request.SubcategoryId;
            dataPackage.MetadataId = request.MetadataId;
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

    }
}
