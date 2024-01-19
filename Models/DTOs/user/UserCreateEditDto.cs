using Microsoft.AspNetCore.Http;
using Models.Enums;

namespace Models.DTOs.user
{
    public class UserCreateEditDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string TitleAbbreviation { get; set; }
        public string Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Active { get; set; }
        public double? CostPerAppointment { get; set; }
        public string RoleId { get; set; }
        public string ParentId { get; set; }
        public string LocalId { get; set; }
    }
}