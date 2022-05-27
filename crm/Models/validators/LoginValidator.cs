using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class LoginValidator : IValidator<string>
    {
        string domen = "@protonmail.com";
        public string Message => "Введен неправильный адрес электронной почты (mymail@protonmail.com)";
        public bool IsValid(string value)
        {
            if (value == null)
                return false;
            string login = value.ToString();
            return login.EndsWith(domen) && login.Length > domen.Length;            
        }
    }
}
