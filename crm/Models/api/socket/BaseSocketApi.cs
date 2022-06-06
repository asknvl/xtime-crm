using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.JsonSerializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.socket
{
    public abstract class BaseSocketApi : ISocketApi
    {
        #region vars       
        Uri uri;
        SocketIO client;
        bool isConnected;

        public event Action<List<usersOnlineDTO>> ReceivedConnectedUsersEvent;
        public event Action<usersDatesDTO> ReceivedUsersDatesEvent;        
        public event Action<userChangedDTO> ReceivedUserInfoChangedEvent;
        #endregion

        public BaseSocketApi(string url)
        {
            uri = new Uri(url);
        }

        #region public
        public virtual async Task Connect(string token)
        {
            client = new SocketIO(uri, new SocketIOOptions()
            {
                ExtraHeaders = new Dictionary<string, string>() {
                    { "Authorization", $"Bearer {token}" }
                }
            });
            //var jsonSerializer = client.JsonSerializer as SystemTextJsonSerializer;
            client.OnConnected += Client_OnConnected;
            client.OnError += Client_OnError;
            client.OnDisconnected += Client_OnDisconnected;

            client.On("connected-users", (response) => {
                usersOnlineDTO[] users = response.GetValue<usersOnlineDTO[]>(1);
                ReceivedConnectedUsersEvent?.Invoke(users.ToList());
            });

            client.On("connected", (response) => {
                usersOnlineDTO user = response.GetValue<usersOnlineDTO>(1);
                var users = new usersOnlineDTO[1] { user };
                ReceivedConnectedUsersEvent?.Invoke(users.ToList());
            });

            client.On("user-activity", (response) => {
                usersDatesDTO dates = response.GetValue<usersDatesDTO>(1);
                ReceivedUsersDatesEvent?.Invoke(dates);
            });

            client.On("user-changed", (response) => {
                userChangedDTO changed = response.GetValue<userChangedDTO>(1);
                ReceivedUserInfoChangedEvent?.Invoke(changed);
            });
            await client.ConnectAsync();
        }

        public async Task Disconnect()
        {
            await client.DisconnectAsync();
        }

        public virtual void RequestConnectedUsers()
        {
            //if (!isConnected)
            //    throw new SocketApiException("Соединение с сервером не установлено (sock)");

            client.EmitAsync("get-connected-users");
        }
        #endregion

        #region callbacks
        private void Client_OnDisconnected(object? sender, string e)
        {
            isConnected = false;
        }
        private void Client_OnError(object? sender, string e)
        {
            isConnected = false;
        }

        private void Client_OnConnected(object? sender, EventArgs e)
        {
            isConnected = true;
        }
        #endregion
    }
}
