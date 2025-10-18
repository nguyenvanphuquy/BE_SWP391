namespace BE_SWP391.Models.DTOs.Request
{
    public class UserRequest
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Organization { get; set; }
        public string RoleName { get; set; }
    }
}
