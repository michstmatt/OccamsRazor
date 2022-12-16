namespace OccamsRazor.Services
{
    using OccamsRazor.Models;
    using OccamsRazor.Repositories;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
 
    public class GameService
    {
        private readonly IGameRepository gameRepository;

        public StateChangedCallback StateChangedDelegate;
        public GameService(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        public Task AddAnswer(PlayerAnswer answer)
        {
            return gameRepository.AddAnswers(new[]{answer});
        }

        public Task OnStateChanged()
        {
            // write gamestate to repository
            return StateChangedDelegate(null);
        }

    }
}