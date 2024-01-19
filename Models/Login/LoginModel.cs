using System.ComponentModel.DataAnnotations;

namespace Models.Login
{
    public class LoginModel
    {
        public string ReturnUrl { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}