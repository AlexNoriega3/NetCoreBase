using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.Enums
{
    public enum GenderEnum
    {
        [Display(Name = "Masculino")]
        [Description("Masculino")]
        Masculino = 1,

        [Display(Name = "Femenino")]
        [Description("Femenino")]
        Femenino = 2,

        [Display(Name = "Otro")]
        [Description("Otro")]
        Otro = 3,
    }
}