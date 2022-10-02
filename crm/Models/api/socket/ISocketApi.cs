using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.socket
{
    public interface ISocketApi
    {
        Task Connect(string token);
        void RequestConnectedUsers();
        Task Disconnect();

        public event Action<List<usersOnlineDTO>> ReceivedConnectedUsersEvent;
        public event Action<usersDatesDTO> ReceivedUsersDatesEvent;
        public event Action<userChangedDTO> ReceivedUserInfoChangedEvent;        
        public event Action<creativeChangedDTO> ReceivedCreativeChangedEvent;
    }

    public class SocketApiException : Exception
    {
        public SocketApiException(string msg) : base(msg) { }
    }
}
