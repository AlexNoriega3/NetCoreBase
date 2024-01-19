using System.ComponentModel.DataAnnotations;

namespace Models.Login
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}