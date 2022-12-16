namespace OccamsRazor.Services
{
    using OccamsRazor.Models;
    using OccamsRazor.Repositories;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using GameWebSockets = System.Collections.Generic.Dictionary<OccamsRazor.Models.Player, System.Net.WebSockets.WebSocket>;
    public delegate Task PlayerAnswerCallback(Player player, AnswerMessage message);
    public delegate Task StateChangedCallback(Game game);

    public class GameConnections
    {
        public Game Game { get; set; }
        public GameWebSockets Sockets { get; set; } = new GameWebSockets();

    }
    public class GameOrchestrationService
    {
        private Dictionary<string, GameConnections> gameConnections = new Dictionary<string, GameConnections>();
        private readonly WebSocketService webSocketService;
        private readonly GameService gameService;

        public GameOrchestrationService(WebSocketService socket, GameService gameService)
        {
            webSocketService = socket;
            this.gameService = gameService;
            webSocketService.AnswerDelegate = OnAnswerReceived;
            gameService.StateChangedDelegate = OnStateChanged;
            gameConnections.Add("1", new GameConnections()
            {
                Game = new Game("1", "", "")
            });
        }

        public async Task OnPlayerConnected(Player player, WebSocket socket)
        {
            if (gameConnections.ContainsKey(player.GameId))
                gameConnections[player.GameId].Sockets.Add(player, socket);
            else
                return;

            var task = new TaskCompletionSource();
            await this.webSocketService.KeepAlive(socket, player, task);
            await task.Task;
            gameConnections[player.GameId].Sockets.Remove(player);
        }

        public Task OnAnswerReceived(Player player, AnswerMessage message)
        {
            var answer = new PlayerAnswer
            {
                Answer = message.Answer,
                GameId = player.GameId,
                PlayerId = player.Id,
            };
            return this.gameService.AddAnswer(answer);
        }
        public async Task OnStateChanged(Game game)
        {
            var gameConnection = gameConnections[game.Id];
            var message = new StateMessage {State = game.State};
            await webSocketService.BroadcastMessage(gameConnection.Sockets, message);
        }

    }
}