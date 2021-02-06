using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Context;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Persistence.Service
{
    public class NotificationService : INotificationService
    {
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

        private WebSocket hostSocket;
        private Dictionary<string, WebSocket> playerSockets;

        public NotificationService()
        {
            playerSockets = new Dictionary<string, WebSocket>();
        }

        public async Task HandleHostConnected(WebSocket socket, TaskCompletionSource<object> t)
        {
            hostSocket = socket;
            await KeepAlive(socket, null, t);
        }


        public async Task HandleConnected(Player player, WebSocket socket, TaskCompletionSource<object> t)
        {
            if (playerSockets.ContainsKey(player.Name))
            {
                //await playerSockets[player.Name].CloseAsync(closeStatus: WebSocketCloseStatus.PolicyViolation, "Duplicate connections", CancellationToken.None);
            }
            playerSockets.Add(player.Name, socket);

            await KeepAlive(socket, player, t);
        }

        private async Task KeepAlive(WebSocket socket, Player p, TaskCompletionSource<object> task)
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
                    if (p != null)
                    {
                        playerSockets.Remove(p.Name);
                    }
                    task.SetResult(null);
                }
            });

        }

        public async Task<bool> SendPlayerMessage(string message)
        {
            var buffer = System.Text.Encoding.ASCII.GetBytes(message);
            foreach (var pair in playerSockets)
            {
                var socket = pair.Value;
                lock (socket)
                {
                    socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            return true;
        }

        public async Task<bool> UpdateHost(string message)
        {
            var buffer = System.Text.Encoding.ASCII.GetBytes(message);
            lock(hostSocket)
            {
                hostSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            return true;
        }
    }
}