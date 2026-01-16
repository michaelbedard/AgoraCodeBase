using System;
using System.Collections.Generic;

namespace _src.Code.Core.Validators
{
    public class UsernameValidator
    {
        // Define any rules for username validation
        private const int MinLength = 5;
        private const int MaxLength = 20;
        private static readonly string[] InvalidUsernames = { "admin", "root", "system" };

        public List<string> ValidationErrors { get; private set; }

        public UsernameValidator()
        {
            ValidationErrors = new List<string>();
        }

        public bool Validate(string username)
        {
            ValidationErrors.Clear();

            // Check length
            if (username.Length < MinLength || username.Length > MaxLength)
            {
                ValidationErrors.Add($"Username must be between {MinLength} and {MaxLength} characters.");
            }

            // Check for invalid characters (allow only letters, digits, and underscores)
            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                ValidationErrors.Add("Username can only contain letters, digits, and underscores.");
            }

            // Check for reserved usernames
            if (Array.Exists(InvalidUsernames, invalidUsername => invalidUsername.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                ValidationErrors.Add("This username is reserved and cannot be used.");
            }

            return ValidationErrors.Count == 0;
        }
    }
}