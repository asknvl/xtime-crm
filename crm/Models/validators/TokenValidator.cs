using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    internal class TokenValidator : IValidator<string>
    {
        public string Message => "Поле ввода токена не может быть пустым";

        public bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
