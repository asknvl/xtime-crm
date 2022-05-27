using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.server
{
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message) { }
    }   

    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
    }

    public class ServerResponseException : ServerException
    {
        public ServerResponseException(HttpStatusCode code) : base($"Ошибка ответа от сервера ({code})")
        {
        }
    }

    public class NoDataReceivedException : ServerException
    {
        public NoDataReceivedException() : base("Не получены данные от сервера")
        {
        }
    }    
}
