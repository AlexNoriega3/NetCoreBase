using Models.DTOs.commons;
using Models.Enums;
using System.Text.Json.Serialization;

namespace Models.DTOs
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TitleAbbreviation { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderEnum Gender { get; set; }

        public string Url { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public double? Rating { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Active { get; set; }
        public double? CostPerAppointment { get; set; }

        public IEnumerable<SelectDto> Department { get; set; }
    }
}