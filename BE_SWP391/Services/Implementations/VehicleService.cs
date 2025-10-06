using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using BE_SWP391.Models.DTOs.Request;
namespace BE_SWP391.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
        public VehicleResponse? GetById(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);
            return vehicle == null ? null : ToResponse(vehicle);
        }
        public IEnumerable<VehicleResponse> GetAll()
        {
            return _vehicleRepository.GetAll().Select(ToResponse);
        }
        public VehicleResponse? Create(VehicleRequest request)
        {
            var vehicle = new Vehicle
            {
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year,
                Type = request.Type,
                RangeKm = request.RangeKm
            };
            _vehicleRepository.Create(vehicle);
            return ToResponse(vehicle);
        }
        public VehicleResponse? Update(int id, VehicleRequest request)
        {
            var vehicle = _vehicleRepository.GetById(id);
            if (vehicle == null)
            {
                return null;
            }
            vehicle.Brand = request.Brand;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;
            vehicle.Type = request.Type;
            vehicle.RangeKm = request.RangeKm;
            _vehicleRepository.Update(vehicle);
            return ToResponse(vehicle);
        }
        public bool Delete(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);
            if (vehicle == null)
            {
                return false;
            }
            _vehicleRepository.Delete(vehicle);
            return true;
        }
        private VehicleResponse ToResponse(Vehicle vehicle)
        {
            return new VehicleResponse
            {
                VehicleId = vehicle.VehicleId,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Type = vehicle.Type,
                RangeKm = vehicle.RangeKm
            };
        }

    }
}
