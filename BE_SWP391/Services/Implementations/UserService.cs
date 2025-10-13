using BCrypt.Net;
using BE_SWP391.Configurations;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Models.Entities;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace BE_SWP391.Services.Implementations
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public readonly JwtTokenGenerator _jwtTokenGenerator;
        public UserService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public UserResponse? GetById(int id)
        {
            var user = _userRepository.GetById(id);
            return user == null ? null : ToResponse(user);
        }


        public IEnumerable<UserResponse> GetAll()
        {
            return _userRepository.GetAll().Select(ToResponse);
        }
        public UserResponse Create(RegisterRequest request)
        {
            if(_userRepository.GetByUsername(request.UserName) != null)
            {
                throw new Exception("Username already exists");
            }
            if (_userRepository.GetByEmail(request.Email) != null)
            {
                throw new Exception("Email already exists");
            }
            var user = new User
            {
                Username = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Email = request.Email,
                FullName = request.FullName,
                Phone = request.Phone,
                Organization = request.Organization,
                Status = "Active",
                RoleId = 3, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _userRepository.Create(user);
            return ToResponse(user);
        }
        //public UserResponse? Update(int id, UpdateUserRequest request)
        //{
        //    var user = _userRepository.GetById(id);
        //    if (user == null) return null;
        //    user.Username = request.UserName ?? user.Username;
        //    user.PasswordHash = request.Password ?? user.PasswordHash;
        //    user.Email = request.Email ?? user.Email;
        //    user.FullName = request.FullName ?? user.FullName;
        //    user.Phone = request.Phone ?? user.Phone;
        //    user.Organization = request.Organization ?? user.Organization;
        //    user.Status = request.Status ?? user.Status;
        //    user.RoleId = request.RoleId ?? user.RoleId;
        //    user.UpdatedAt = DateTime.UtcNow;
        //    _userRepository.Update(user);
        //    return ToResponse(user);
        //}
        public bool Delete(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return false;
            _userRepository.UpdateStatus(id, "Inactive");
            return true;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            var user = _userRepository.GetByUsername(request.Username);
            if (user == null) return null;
            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValid) return null;
            var token = _jwtTokenGenerator.GenerateToken(user);
            return new LoginResponse
            {
                Token = token,
                RefreshToken = "",
                Expires = DateTime.UtcNow.AddHours(1),
                Username = user.Username,
                Email = user.Email,
                RoleId = user.RoleId,
            };

        }
       
        public static UserResponse ToResponse(User user) => new UserResponse
        {
            UserId = user.UserId.ToString(),
            UserName = user.Username,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            FullName = user.FullName ?? string.Empty,
            Phone = user.Phone ?? string.Empty,
            OrganizationId = user.Organization ?? string.Empty,
            Status = user.Status,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt ?? DateTime.MinValue,
            UpdatedAt = user.UpdatedAt ?? DateTime.MinValue
        };
    }
}
