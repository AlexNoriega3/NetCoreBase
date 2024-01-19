using Models.DTOs.commons;
using Models.Enums;
using System.Text.Json.Serialization;

namespace Models.DTOs.user
{
    public class ProviderProfileDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TitleAbbreviation { get; set; }
        public string Url { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderEnum Gender { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public double? CostPerAppointment { get; set; }
        public double? Rating { get; set; }
        public IEnumerable<SelectDto> LevelLocal { get; set; }
        public IEnumerable<SelectDto> Department { get; set; }
        public IEnumerable<SelectDto> SubDepartments { get; set; }
    }
}