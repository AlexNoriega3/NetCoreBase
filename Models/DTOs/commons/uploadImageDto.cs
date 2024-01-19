using Microsoft.AspNetCore.Http;

namespace Models.DTOs.commons
{
    public class uploadImageDto
    {
        public string id { get; set; }
        public IFormFile image { get; set; }
    }
}