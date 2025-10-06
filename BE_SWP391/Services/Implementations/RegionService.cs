using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models.DTOs.Request;
using System.Collections.Generic;
namespace BE_SWP391.Services.Implementations
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository regionRepository;
        public RegionService(IRegionRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }
        public RegionResponse? GetById(int id)
        {
            var region = regionRepository.GetById(id);
            return region == null ? null : ToResponse(region);
        }
        public IEnumerable<RegionResponse> GetAll()
        {
            return regionRepository.GetAll().Select(ToResponse);
        }
        public RegionResponse? Create(RegionRequest request)
        {
            var region = new Region
            {
                RegionName = request.RegionName,
                Country = request.Country,
                City = request.City,
                Description = request.Description
            };
            regionRepository.Create(region);
            return ToResponse(region);
        }
        public RegionResponse? Update(int id, RegionRequest request)
        {
            var region = regionRepository.GetById(id);
            if (region == null)
            {
                return null;
            }
            region.RegionName = request.RegionName;
            region.Country = request.Country;
            region.City = request.City;
            region.Description = request.Description;
            regionRepository.Update(region);
            return ToResponse(region);
        }
        public bool Delete(int id)
        {
            var region = regionRepository.GetById(id);
            if (region == null)
            {
                return false;
            }
            regionRepository.Delete(region);
            return true;
        }
        public static RegionResponse ToResponse(Region region)
        {
            return new RegionResponse
            {
                RegionId = region.RegionId,
                RegionName = region.RegionName,
                Country = region.Country,
                City = region.City,
                Description = region.Description
            };
        }
    }
}
