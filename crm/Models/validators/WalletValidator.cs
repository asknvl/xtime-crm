using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class WalletValidator : IValidator<string>
    {
        public string Message => "Укажите адрес USDT TRC20 кошелька";

        public bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
