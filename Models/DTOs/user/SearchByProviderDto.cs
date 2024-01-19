using System.ComponentModel.DataAnnotations;

namespace Models.DTOs.user
{
    public class SearchByProviderDto : QueryParameters
    {
        [Required]
        public string providerId { get; set; }
    }
}