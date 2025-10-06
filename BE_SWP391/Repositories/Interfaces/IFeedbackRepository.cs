using BE_SWP391.Models.Entities;

namespace BE_SWP391.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Feedback? GetById(int id);
        IEnumerable<Feedback> GetAll();
        void Create(Feedback feedback);
        void Update(Feedback feedback);
        void Delete(Feedback feedback);
    }
}
