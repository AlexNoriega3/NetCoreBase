using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class QueryParameters
    {
        private int _pageSize = 15;

        [Required]
        public int Page { get; set; } = 1;

        public string Search { get; set; }

        [Required]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}