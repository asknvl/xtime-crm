using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class TelegramValidator : IValidator<string>
    {
        public string Message => "Неверно задано имя Telegram (имя должно начнаться с @, может содержать буквы, цифры, подчеркивание\n@telegramname_)";

        public bool IsValid(string value)
        {           
            Regex regex = new Regex(@"^[@][[a-zA-Z0-9_]{5,32}$");
            return regex.IsMatch(value);
        }
    }
}
