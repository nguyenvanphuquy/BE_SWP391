namespace BE_SWP391.Models.DTOs.Request
{
    public class RegisterRequest
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String FullName { get; set; }
        public String Organization { get; set; }

        public int RoleId { get; set; }


    }
}
