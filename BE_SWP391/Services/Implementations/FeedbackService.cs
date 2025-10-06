using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Models;
namespace BE_SWP391.Services.Implementations
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }
        public FeedbackResponse? GetById(int id)
        {
            var feedback = _feedbackRepository.GetById(id);
            return feedback == null ? null : ToResponse(feedback);
        }
        public IEnumerable<FeedbackResponse> GetAll()
        {
            return _feedbackRepository.GetAll().Select(f => ToResponse(f));
        }
        public FeedbackResponse Create(FeedbackRequest request)
        {
            var feedback = new Feedback
            {
                Rating = request.Rating,
                Title = request.Title,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = request.UserId,
                PackageId = request.PackageId
            };
            _feedbackRepository.Create(feedback);
            return ToResponse(feedback);
        }
        public FeedbackResponse? Update(int id, FeedbackRequest request)
        {
            var feedback = _feedbackRepository.GetById(id);
            if (feedback == null)
            {
                return null;
            }
            feedback.Rating = request.Rating;
            feedback.Title = request.Title;
            feedback.Comment = request.Comment;
            feedback.UpdatedAt = DateTime.UtcNow;
            feedback.UserId = request.UserId;
            feedback.PackageId = request.PackageId;
            _feedbackRepository.Update(feedback);
            return ToResponse(feedback);
        }
        public bool Delete(int id)
        {
            var feedback = _feedbackRepository.GetById(id);
            if (feedback == null) return false;
            _feedbackRepository.Delete(feedback);
            return true;
        }
        public static FeedbackResponse ToResponse(Feedback feedback)
        {
            return new FeedbackResponse
            {
                FeedbackId = feedback.FeedbackId,
                Rating = feedback.Rating,
                Title = feedback.Title,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt,
                UpdatedAt = feedback.UpdatedAt,
                UserId = feedback.UserId,
                PackageId = feedback.PackageId

            };
        }
    }
}
