using Generify.Services.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generify.Services.Security
{
    public class PasswordValidator : IPasswordValidator
    {
        private const int _minLength = 6;

        private readonly List<char> _specialCharacters = new List<char>
        {
            '@',
            '#',
            '!',
            '~',
            '$',
            '%',
            '^',
            '&',
            '*',
            '(',
            ')',
            '-',
            '+',
            '/',
            ':',
            '.',
            ',',
            '<',
            '>',
            '?',
            '|'
        };

        public string ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return "The password is required";
            }

            if (password.Length < _minLength)
            {
                return $"The password must contain at least {_minLength} characters";
            }

            if (password.Contains(' '))
            {
                return "The password may not contain any white space";
            }

            if (!password.Any(o => char.IsDigit(o)))
            {
                return "The password must contain digits";
            }

            if (!password.Any(o => _specialCharacters.Contains(o)))
            {
                return $"The password must contain special characters ({string.Join(", ", _specialCharacters)})";
            }

            if (!password.Any(o => char.IsLower(o)) || !password.Any(o => char.IsUpper(o)))
            {
                return "The password must contain upper and lower case letters";
            }

            return null;
        }
    }
}
