using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.autocompletions
{
    public class EmailAutoComplete : IAutoComplete
    {
        public string AutoComplete(string input)
        {
            if (input.Contains("@"))
            {
                int ind = input.IndexOf("@");
                input = input.Substring(0, ind);
                input = input + "@protonmail.com";
            }

            return input;
        }
    }
}
