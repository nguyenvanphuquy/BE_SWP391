using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
namespace BE_SWP391.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly EvMarketContext _context;
        public VehicleRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Vehicle? GetById (int id)
        {
            return _context.Vehicles.Find(id);
        }
        public IEnumerable<Vehicle> GetAll ()
        {
            return _context.Vehicles.ToList();
        }
        public void Create (Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }
        public void Update (Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }
        public void Delete (Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }

    }
}
