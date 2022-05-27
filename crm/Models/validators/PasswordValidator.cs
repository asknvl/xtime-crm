using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class PasswordValidator : IValidator<string>
    {
#if DEBUG
        const int min_length = 2;
        const bool isSimple = true;
#else
        const int min_length = 12;
        const bool isSimple = false;
#endif
        public string Message => "Пароль должен состоять не менее, " +
            "чем из 12 символов и содрежать в себе буквы, числа и знаки (%,+,-,^,...)";

        public bool IsValid(string value)
        {
            if (value == null)
                return false;

            string password = value.ToString();
            if (password.Length < min_length)
                return false;

            bool result = false;

            if (!isSimple)
            {
                bool isLetter = password.Any(c => char.IsLetter(c));
                bool isDigit = password.Any(c => char.IsDigit(c));
                bool isSymbol = password.Any(c => char.IsSymbol(c));
                result = isLetter && isDigit && isSymbol;
            } else
                result = true;


            return result;
        }
    }
}
