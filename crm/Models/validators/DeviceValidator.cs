using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public class DeviceValidator : IValidator<string>
    {
        public string Message => "Введите модель рабочего устройства";

        public bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
