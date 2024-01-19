namespace Models.DTOs
{
    public class AuthResponseDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Url { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
        public List<string> Roles { get; set; }
    }
}