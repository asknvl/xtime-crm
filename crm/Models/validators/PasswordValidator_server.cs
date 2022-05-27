using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class PasswordValidator_server : IValidator<string>
    {
        public string Message => "Пароль должен состоять не менее, " +
            "чем из 12 символов и содрежать в себе числа, строчные и заглавные буквы";

        public bool IsValid(string value)
        {
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[a-zA-Z\d^@$.!%*#?&]");
            return regex.IsMatch(value);
        }
    }
}
