using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class FullNameValidator : IValidator<string>
    {
        public string Message => "Неправильный формат ФИО (Фамилия Имя Очество)";

        public bool IsValid(string value)
        {
            Regex regex = new Regex(@"^[A-ЯЁ][а-яё]+\s[A-ЯЁ][а-яё]+\s[A-ЯЁ][а-яё]+$");
            return regex.IsMatch(value);           
        }
    }
}
