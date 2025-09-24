using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;

namespace BE_SWP391.Services.Interfaces
{
    public interface IUserService
    {
        UserResponse? GetById(int id);
        IEnumerable<UserResponse> GetAll();
        UserResponse Create(RegisterRequest request);
        //ApiResponse Update(UserResponse user);
        bool Delete(int id);
        LoginResponse? Login(LoginRequest request);
    }
}
