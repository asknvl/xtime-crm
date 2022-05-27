using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    internal class PhoneNumberValidator : IValidator<string>
    {
        public string Message => $"Неправильный формат номер телефона \n+7 (925) 000-00-00";

        public bool IsValid(string value)
        {
            Regex regex = new Regex(@"^[+][7][ ][(][0-9]{3}[)][ ][0-9]{3}[-][0-9]{2}[-][0-9]{2}$");
            return regex.IsMatch(value);
        }
    }
}
