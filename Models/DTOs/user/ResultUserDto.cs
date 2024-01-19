using Models.Enums;
using System.Text.Json.Serialization;

namespace Models.DTOs.user
{
    public class ResultUserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TitleAbbreviation { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderEnum Gender { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public double? Rating { get; set; }

        public DateTime VisitDate { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
    }
}