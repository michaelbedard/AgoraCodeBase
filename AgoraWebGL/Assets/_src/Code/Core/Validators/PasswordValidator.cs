using System.Collections.Generic;

namespace _src.Code.Core.Validators
{
    public class PasswordValidator
    {
        private const int MinLength = 3;
        private const int MaxLength = 128;

        // Define what special characters are allowed (you can modify this as needed)
        private static readonly char[] SpecialCharacters = "!@#$%^&*()_+-=[]{}|;:'\",.<>?/".ToCharArray();

        // Common password patterns that should be avoided (you can customize this list)
        private static readonly string[] BannedPasswords = { "password", "123456", "qwerty", "admin" };

        public List<string> ValidationErrors { get; private set; }

        public PasswordValidator()
        {
            ValidationErrors = new List<string>();
        }

        // Main validation method
        public bool Validate(string password)
        {
            ValidationErrors.Clear();

            // Check length
            if (password.Length < MinLength || password.Length > MaxLength)
            {
                ValidationErrors.Add($"Password must be between {MinLength} and {MaxLength} characters long.");
            }

            // // Check for an uppercase letter
            // if (!password.Any(char.IsUpper))
            // {
            //     ValidationErrors.Add("Password must contain at least one uppercase letter.");
            // }
            //
            // // Check for a lowercase letter
            // if (!password.Any(char.IsLower))
            // {
            //     ValidationErrors.Add("Password must contain at least one lowercase letter.");
            // }
            //
            // // Check for a digit
            // if (!password.Any(char.IsDigit))
            // {
            //     ValidationErrors.Add("Password must contain at least one digit.");
            // }
            //
            // // Check for a special character
            // if (!password.Any(c => SpecialCharacters.Contains(c)))
            // {
            //     ValidationErrors.Add($"Password must contain at least one special character ({new string(SpecialCharacters)}).");
            // }
            //
            // // Check for banned/common passwords
            // if (BannedPasswords.Contains(password, StringComparer.OrdinalIgnoreCase))
            // {
            //     ValidationErrors.Add("This password is too common and cannot be used.");
            // }

            return ValidationErrors.Count == 0;
        }
    }
}