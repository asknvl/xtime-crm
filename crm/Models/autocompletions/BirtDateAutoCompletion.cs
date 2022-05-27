using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.autocompletions
{
    public class BirtDateAutoCompletion : IAutoComplete
    {
        public string AutoComplete(string input)
        {
            //switch (input.Length)
            //{
            //    case 2:
            //        input = input + ".";
            //        break;
            //    case 5:
            //        input = input + ".";
            //        break;
            //    default:
            //        break;

            //}
            return input;
        }
    }
}
