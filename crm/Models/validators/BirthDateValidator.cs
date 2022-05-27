using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class BirthDateValidator : IValidator<string>
    {
        public string Message => "Неправильный формат даты рождения (дд.мм.гггг)";

        public bool IsValid(string value)
        {
            Regex regex = new Regex(@"(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)\d\d");
            return regex.IsMatch(value) && value.Length == 10;
        }
    }
}
