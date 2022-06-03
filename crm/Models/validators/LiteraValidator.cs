using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    internal class LiteraValidator : IValidator<string>
    {
        public string Message => "Неверно заданы буквы пользователя (должны быть две заглавные латинские буквы, например AB)";

        public bool IsValid(string value)
        {
            Regex regex = new Regex(@"^[[A-Z0-9]{2,2}$");
            return regex.IsMatch(value);
        }
    }
}
