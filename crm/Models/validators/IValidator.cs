using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.validators
{
    public interface IValidator<T>
    {
        string Message { get; }
        bool IsValid(T value);        
    }
}
