using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MovieApi
{
    public static class ModelValidationExtension
    {
        public static bool IsValidate(this ModelStateDictionary model, object req)
        {            
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(req, null, null);
            Validator.TryValidateObject(req, validationContext, results, true);
            if (model is IValidatableObject) (req as IValidatableObject).Validate(validationContext);
            return (results.Count() == 0) ? true : false;
        }
    }
}