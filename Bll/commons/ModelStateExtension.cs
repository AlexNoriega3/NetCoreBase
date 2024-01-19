using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bll.commons
{
    public static class ModelStateExtension
    {
        public static string GetErrors(this ModelStateDictionary model)
            => string.Join("\n => ", model.Values
            .SelectMany(x => x.Errors)
            .Select(x => x.Exception + x.ErrorMessage));
    }
}