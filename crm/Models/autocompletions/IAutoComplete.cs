using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.autocompletions
{
    public interface IAutoComplete
    {
        string AutoComplete(string input);
    }
}
