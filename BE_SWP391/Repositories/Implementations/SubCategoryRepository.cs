using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;

namespace BE_SWP391.Repositories.Implementations
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly EvMarketContext _context;
        public SubCategoryRepository(EvMarketContext context)
        {
            _context = context;
        }
        public SubCategory? GetById(int id)
        {
            return _context.SubCategorys.Find(id);
        }
        public IEnumerable<SubCategory> GetAll()
        {
            return _context.SubCategorys.ToList();
        }
        public void Create(SubCategory subCategory)
        {
            _context.SubCategorys.Add(subCategory);
            _context.SaveChanges();
        }
        public void Update(SubCategory subCategory)
        {
            _context.SubCategorys.Update(subCategory);
            _context.SaveChanges();
        }
        public void Delete(SubCategory subCategory)
        {
            _context.SubCategorys.Remove(subCategory);
            _context.SaveChanges();
        }
        public SubCategory GetByName(string name)
        {
            return _context.SubCategorys.FirstOrDefault(x => x.SubcategoryName == name);
        }


    }
}
