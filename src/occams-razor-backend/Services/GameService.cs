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
    public delegate void HandleAnswerMessage (Player player, AnswerMessage message);

    public class GameConnections
    {
        public Game Game {get; set;}
        public GameWebSockets Sockets {get; set;} = new GameWebSockets();

    }
    public class GameService
    {
        private Dictionary<string, GameConnections> games = new Dictionary<string, GameConnections>();
        private readonly WebSocketService webSocketService;
        private readonly IGameRepository gameRepository;

        public GameService(WebSocketService socket, IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
            webSocketService = socket;
            webSocketService.AnswerDelegate = OnAnswerReceived;
            games.Add("1", new GameConnections(){
                Game = new Game("1", "", "")
            });
        }

        public async Task OnPlayerConnected(Player player, WebSocket socket)
        {
            if (games.ContainsKey(player.GameId))
                games[player.GameId].Sockets.Add(player, socket);
            else
                return;

            var task = new TaskCompletionSource();
            await this.webSocketService.KeepAlive(socket, player, task);
            await task.Task;
            games[player.GameId].Sockets.Remove(player);
        }

        public Task OnAnswerReceived(Player player, AnswerMessage message)
        {
            var answer = new PlayerAnswer
            {
                Answer = message.Answer,
                GameId = player.GameId,
                PlayerId = player.Id,
                QuestionId = 0
            };
        }
        public async Task OnStateChanged(string gameId, StateMessage message)
        {
            var game = games[gameId];
            game.Game.State = message.State;
            await webSocketService.BroadcastMessage(game.Sockets, message);
        }

    }
}