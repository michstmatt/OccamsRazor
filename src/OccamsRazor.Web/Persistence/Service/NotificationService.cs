using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Persistence.Service
{

    public class NotificationContext
    {
        public Dictionary<string, WebSocket> Sockets { get; set; }
    }

    public class NotificationService : INotificationService
    {
        private const string HostId = "Host";
        private static NotificationService _service;
        public static NotificationService singleton
        {
            get
            {
                if (_service == null)
                {
                    _service = new NotificationService();
                }
                return _service;
            }
        }

        private Dictionary<int, NotificationContext> servers;

        private NotificationContext getContext(int gameId)
        {
            if (!servers.ContainsKey(gameId))
            {
                servers.Add(gameId, new NotificationContext
                {
                    Sockets = new Dictionary<string, WebSocket>()
                });
            }
            return servers[gameId];
        }

        public NotificationService()
        {
            servers = new Dictionary<int, NotificationContext>();
        }

        public async Task HandleHostConnected(int gameId, WebSocket socket, TaskCompletionSource<object> t)
        {
            var server = this.getContext(gameId);

            server.Sockets.Add(HostId, socket);
            
            await KeepAlive(gameId, socket, new Player{Name = HostId}, t);
        }


        public async Task HandleConnected(int gameId, Player player, WebSocket socket, TaskCompletionSource<object> t)
        {
            var server = this.getContext(gameId);
            if (server.Sockets.ContainsKey(player.Name))
            {
                //await playerSockets[player.Name].CloseAsync(closeStatus: WebSocketCloseStatus.PolicyViolation, "Duplicate connections", CancellationToken.None);
            }
            server.Sockets.Add(player.Name, socket);

            await KeepAlive(gameId, socket, player, t);
        }

        private async Task KeepAlive(int gameId, WebSocket socket, Player p, TaskCompletionSource<object> task)
        {

            bool run = true;
            var buffer = new byte[1024];
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    while (run)
                    {
                        var response = await socket.ReceiveAsync(buffer, CancellationToken.None);
                        run = (response.MessageType != WebSocketMessageType.Close);
                        task.Task.Wait(1000);
                    }
                }
                finally
                {
                    var server = this.getContext(gameId);
                    if (p != null)
                    {
                        server.Sockets.Remove(p.Name);
                    }
                    task.SetResult(null);
                }
            });

        }

        public async Task<bool> SendPlayerMessage(int gameId, string message)
        {
            var server = this.getContext(gameId);
            var buffer = System.Text.Encoding.ASCII.GetBytes(message);
            foreach (var pair in server.Sockets)
            {
                if(pair.Key == HostId)
                    continue;

                var socket = pair.Value;
                lock (socket)
                {
                    socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            return true;
        }

        public async Task<bool> UpdateHost(int gameId, string message)
        {
            var server = this.getContext(gameId);
            var hostSocket = server.Sockets.GetValueOrDefault(HostId);
            if (hostSocket == null)
                return false;

            var buffer = System.Text.Encoding.ASCII.GetBytes(message);
            lock(hostSocket)
            {
                hostSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            return true;
        }
    }
}