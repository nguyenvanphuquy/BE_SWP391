using System;
using System.Collections.Generic;
using System.Linq;
using BE_SWP391.Models.Entities;
namespace BE_SWP391.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Vehicle? GetById(int id);
        IEnumerable<Vehicle> GetAll();
        void Create(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);

    }
}
