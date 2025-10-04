using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Models.Entities;
using BE_SWP391.Data;

namespace BE_SWP391.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EvMarketContext _context;
        public CategoryRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Category? GetById(int id)
        {
            return _context.Categorys.Find(id);
        }
        public IEnumerable<Category> GetAll()
        {
            return _context.Categorys.ToList();
        }
        public void Create(Category category)
        {
            _context.Categorys.Add(category);
            _context.SaveChanges();
        }
        public void Update(Category category)
        {
            _context.Categorys.Update(category);
            _context.SaveChanges();
        }
        public void Delete(Category category)
        {
                _context.Categorys.Remove(category);
                _context.SaveChanges();
       
        }
    }
}
