using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.JsonSerializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace crm.Models.api.socket
{
    public abstract class BaseSocketApi : ISocketApi
    {
        #region vars       
        Uri uri;
        SocketIO client;
        Timer timer = new Timer();
        bool isConnected;

        public event Action<List<usersOnlineDTO>> ReceivedConnectedUsersEvent;
        public event Action<usersDatesDTO> ReceivedUsersDatesEvent;        
        public event Action<userChangedDTO> ReceivedUserInfoChangedEvent;
        #endregion

        public BaseSocketApi(string url)
        {
            uri = new Uri(url);
            timer.AutoReset = true;
            timer.Interval = 10000;
            timer.Elapsed += Timer_Elapsed;
        }

        #region public
        public virtual async Task Connect(string token)
        {
            client = new SocketIO(uri, new SocketIOOptions()
            {
                ExtraHeaders = new Dictionary<string, string>() {
                    { "Authorization", $"Bearer {token}" }
                },

                Reconnection = true,
                ReconnectionDelay = 5000,
                ReconnectionAttempts = 10                

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
            
            timer.Start();
        }

        private async void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                 await client.EmitAsync("keep-alive");
                 
            } catch (Exception)
            {
                try
                {
                    await client.ConnectAsync();
                } catch { };

            }

        }

        public async Task Disconnect()
        {
            timer.Elapsed -= Timer_Elapsed;
            timer.Stop();            
            await client.DisconnectAsync();
        }

        public virtual void RequestConnectedUsers()
        {
            client.EmitAsync("get-connected-users");
        }
        #endregion

        #region callbacks
        private void Client_OnDisconnected(object? sender, string e)
        {
            Debug.WriteLine("disc");
        }
        private void Client_OnError(object? sender, string e)
        {
            Debug.WriteLine("err");
        }

        private void Client_OnConnected(object? sender, EventArgs e)
        {
            RequestConnectedUsers();
            Debug.WriteLine("conn");
        }
        #endregion
    }
}
