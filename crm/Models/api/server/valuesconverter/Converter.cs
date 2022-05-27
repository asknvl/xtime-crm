using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.server.valuesconverter
{
    public class Converter : IConverter
    {
        public string date(string date, Direction direction)
        {
            string res = "";
            string[] splt;
            switch (direction)
            {                
                case Direction.user_server:
                    splt = date.Split('.');
                    res = $"{splt[2]}-{splt[1]}-{splt[0]}";
                    break;
                case Direction.server_user:
                    splt = date.Split('-');
                    res = $"{splt[2]}.{splt[1]}.{splt[0]}";
                    break;
                default:
                    break;
            }
            return res;
        }

        public string phone(string phone, Direction direction)
        {
            string res = "";
            switch (direction)
            {
                case Direction.user_server:
                    res = phone
                        .Replace("+", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Replace(" ", "")
                        .Replace("-", "");

                    break;
                case Direction.server_user:                    
                    string p = phone;
                    res = $"+{p[0]} ({p[1]}{p[2]}{p[3]}) {p[4]}{p[5]}{p[6]}-{p[7]}{p[8]}-{p[9]}{p[10]}";
                    break;
                default:
                    break;
            }
            return res;
        }

        public string telegram(string telegram, Direction direction)
        {
            string res = "";
            switch (direction)
            {
                case Direction.user_server:
                    res = telegram.Replace("@", "");
                    break;
                case Direction.server_user:
                    res = $"@{telegram}";
                    break;
                default:
                    break;
            }
            return res;
        }
    }
}
