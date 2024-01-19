using Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.DTOs.user
{
    public class VisitorDto
    {
        [Required]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "The field {0} required length of 10.")]
        public string Phone { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "The field {0} required length of 100.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "The field {0} required length of 100.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime? BirthDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderEnum Gender { get; set; }
    }
}