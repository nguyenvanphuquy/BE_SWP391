using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.DTOs.Request;
namespace BE_SWP391.Services.Interfaces
{
    public interface IFeedbackService
    {
        FeedbackResponse? GetById(int id);
        IEnumerable<FeedbackResponse> GetAll();
        FeedbackResponse Create(FeedbackRequest request);
        FeedbackResponse? Update(int id, FeedbackRequest request);
        bool Delete(int id);
    }
}
