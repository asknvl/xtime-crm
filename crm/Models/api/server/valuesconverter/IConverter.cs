using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.server.valuesconverter
{
    public enum Direction
    {
        server_user,
        user_server
    }
    public interface IConverter
    {
        string date(string date, Direction direction);
        string phone(string phone, Direction direction);
        string telegram(string telegram, Direction direction);
    }
}
