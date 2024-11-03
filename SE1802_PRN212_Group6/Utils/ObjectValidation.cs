using System.ComponentModel.DataAnnotations;

namespace SE1802_PRN212_Group6.Utils
{
    public static class ObjectValidation
    {
        public static bool TryValidate(this object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

            if (!isValid)
            {
                var errorMessages = validationResults
                    .Select(vr => vr.ErrorMessage)
                    .ToList();

                Dialog.ShowError(string.Join("\n", errorMessages.Select(msg => $"• {msg}")));
            }

            return isValid;
        }
    }
}
