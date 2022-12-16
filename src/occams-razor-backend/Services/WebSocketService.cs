namespace OccamsRazor.Services
{
    using OccamsRazor.Models;

    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using GameWebSockets = System.Collections.Generic.Dictionary<OccamsRazor.Models.Player, System.Net.WebSockets.WebSocket>;

    public class WebSocketService
    {
        public PlayerAnswerCallback AnswerDelegate;
        public async Task KeepAlive(WebSocket socket, Player player, TaskCompletionSource task)
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
                        await this.AcceptMessage(player, buffer, response.Count, player.Role == PlayerRole.GameAdmin);
                        task.Task.Wait(1000);
                    }
                }
                finally
                {
                    task.SetResult();
                }
            });
        }
        private async Task<AbstractMessage> deserializeMessage(Stream s)
        {
            MessageType code = (MessageType)(s.ReadByte() - '0');
            Type t = code.GetMessageType();
            AbstractMessage message = (AbstractMessage) await JsonSerializer.DeserializeAsync(s, t);
            return message;
        }

        public async Task AcceptMessage(Player player, byte[] data, int count, bool isHost = false)
        {
            using var ms = new MemoryStream(data, 0, count);

            var message = await deserializeMessage(ms);
            if (message is AnswerMessage)
            {
                await AnswerDelegate(player, message as AnswerMessage);
            }
        }

        public async Task BroadcastMessage (GameWebSockets game, AbstractMessage message)
        {
            var stream = await message.ToJson();
            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer);
            foreach(var (player,socket) in game)
            {
                await socket.SendAsync(buffer, WebSocketMessageType.Text, false, System.Threading.CancellationToken.None);
            }
        }


    }
}