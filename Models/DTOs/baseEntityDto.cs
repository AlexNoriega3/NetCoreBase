using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.DTOs
{
    public abstract class baseEntityDto
    {
        [JsonIgnore]
        public string CreateBy { get; set; }

        [JsonIgnore]
        public DateTime? CreateDate { get; set; }

        [JsonIgnore]
        public bool Active { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int IsActive => Active ? 1 : 0;
    }
}