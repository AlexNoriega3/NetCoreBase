using System.ComponentModel.DataAnnotations;

namespace Models.Login
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}