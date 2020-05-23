using System.Globalization;
using System.Windows.Controls;

namespace Mohammad.Wpf.ValidationRules
{
    public class MandatoryValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var isValid = value != null && !string.IsNullOrEmpty(value.ToString());
            return new ValidationResult(isValid, "Hello");
        }
    }
}