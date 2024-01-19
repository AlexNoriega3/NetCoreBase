using System.ComponentModel.DataAnnotations;

namespace Models.DTOs
{
    public class LoginDto
    //: ActionFilterAttribute, IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1} characters",
            MinimumLength = 6)]
        public string Password { get; set; }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //	if (!context.ModelState.IsValid)
        //	{
        //		context.Result = new BadRequestObjectResult(context.ModelState);
        //	}
        //}

        //      public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //      {
        //	yield return new ValidationResult(errorMessage: "Errores");

        //}
    }
}