using Models.DTOs.commons;

namespace Models.DTOs.user
{
    public class MostVisitedDto
    {
        public string Id { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double? Rating { get; set; }
        public IEnumerable<SelectDto>? Department { get; set; }
        public IEnumerable<SelectDto> SubDepartments { get; set; }
    }
}