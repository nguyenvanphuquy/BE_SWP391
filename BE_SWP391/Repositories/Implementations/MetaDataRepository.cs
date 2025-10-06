using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BE_SWP391.Repositories.Implementations
{
    public class MetaDataRepository : IMetaDataRepository
    {
        private readonly EvMarketContext _context;
        public MetaDataRepository(EvMarketContext context)
        {
            _context = context;
        }
        public MetaData? GetById(int id)
        {
            return _context.MetaDatas.Find(id);
        }
        public IEnumerable<MetaData> GetAll()
        {
            return _context.MetaDatas.ToList();
        }
        public void Create(MetaData metadata)
        {
            _context.MetaDatas.Add(metadata);
            _context.SaveChanges();
        }
        public void Update(MetaData metadata)
        {
            _context.MetaDatas.Update(metadata);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var metadata = _context.MetaDatas.Find(id);
            if (metadata != null)
            {
                _context.MetaDatas.Remove(metadata);
                _context.SaveChanges();
            }
        }

        
    }
}
