using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace cvs.ValidationRules
{
    internal class TimeStringValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = value as string;
            if (Regex.IsMatch(text, @"\d{1,2}:\d{1,2}"))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Text is not a matching time string");
            }
        }
    }
}
