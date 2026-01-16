using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace _src.Code.Core.Validators
{
    public class EmailValidator
    {
        // Regex pattern to validate an email lll
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public List<string> ValidationErrors { get; private set; }

        public EmailValidator()
        {
            ValidationErrors = new List<string>();
        }

        // Main validation method
        public bool Validate(string email)
        {
            ValidationErrors.Clear();

            // Check if email is empty
            if (string.IsNullOrWhiteSpace(email))
            {
                ValidationErrors.Add("Email cannot be empty.");
                return false;
            }

            // Check if email matches regex pattern
            if (!EmailRegex.IsMatch(email))
            {
                ValidationErrors.Add("Email format is invalid.");
                return false;
            }

            return true;
        }
    }

}