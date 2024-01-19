using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public abstract class baseEntity
    {
        [Display(AutoGenerateField = false)]
        [ScaffoldColumn(false)]
        public string CreateBy { get; set; }

        [ScaffoldColumn(false)]
        public int? UpdateBy { get; set; }

        [ScaffoldColumn(false)]
        public int? DeleteBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DTUpdateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DeleteDate { get; set; }

        [ScaffoldColumn(false)]
        public bool Active { get; set; }

        [ScaffoldColumn(false)]
        [NotMapped]
        public string UpdateDate => DTUpdateDate != null && DTUpdateDate.Value != null
            ? DTUpdateDate.Value.ToString("dd/MM/yyyy") : CreateDate.ToString("dd/MM/yyyy");

        [ScaffoldColumn(false)]
        [NotMapped]
        public int IsActive => Active ? 1 : 0;
    }
}