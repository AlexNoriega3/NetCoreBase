namespace Models.DTOs.user
{
    public class UserPostDto : LoginDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string RoleName { get; set; }
    }
}