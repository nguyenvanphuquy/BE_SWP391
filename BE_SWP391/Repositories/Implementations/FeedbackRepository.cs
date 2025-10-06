using BE_SWP391.Data;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
namespace BE_SWP391.Repositories.Implementations
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly EvMarketContext _context;
        public FeedbackRepository(EvMarketContext context)
        {
            _context = context;
        }
        public Feedback? GetById(int id)
        {
            return _context.Feedbacks.Find(id);
        }
        public IEnumerable<Feedback> GetAll()
        {
            return _context.Feedbacks.ToList();
        }
        public void Create(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
        }
        public void Update(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            _context.SaveChanges();
        }
        public void Delete(Feedback feedback)
        {
                _context.Feedbacks.Remove(feedback);
                _context.SaveChanges();
        }

    }
}
