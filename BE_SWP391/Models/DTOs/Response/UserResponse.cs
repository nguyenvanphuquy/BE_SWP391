using Microsoft.Identity.Client;

namespace BE_SWP391.Models.DTOs.Response
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; } = null;
        public string PasswordHash { get; set; } = null;
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone {  get; set; }
        public string OrganizationId { get; set; }
        public string Status { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }




    }
}
