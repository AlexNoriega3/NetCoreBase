using Microsoft.AspNetCore.Http;
using Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.DTOs.user
{
    public class UserPutDto
    {
        public IFormFile imageFile { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 300, MinimumLength = 1, ErrorMessage = "The field {0} required length of 100.")]
        public string name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "The field {0} required length of 100.")]
        public string firstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "The field {0} required length of 100.")]
        public string lastName { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "The field {0} max length is 500.")]
        public string Url { get; set; }

        [StringLength(maximumLength: 100, ErrorMessage = "The field {0} max length is 100.")]
        public string countryCode { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "The field {0} max length is 50.")]
        public string titleAbbreviation { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderEnum gender { get; set; }

        [Required]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "The field {0} required length of 10.")]
        public string phone { get; set; }

        [StringLength(maximumLength: 150, ErrorMessage = "The field {0} max length is 150.")]
        public string country { get; set; }

        [StringLength(maximumLength: 150, ErrorMessage = "The field {0} max length is 150.")]
        public string city { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "The field {0} max length is 200.")]
        public string address { get; set; }

        public DateTime? birthDate { get; set; }

        [Range(0.0, 100000.00, ErrorMessage = "The field {0} must be positive and less than or equal to 100,000.00.")]
        public double? costPerAppointment { get; set; }
    }
}