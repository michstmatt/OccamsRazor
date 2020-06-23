using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Context;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Persistence.Service
{
    public class GameDataService : IGameDataService
    {
        private IGameDataRepository Repository;
        public GameDataService(IGameDataRepository repository)
        {
            Repository = repository;
        }

        public Task<GameMetadata> SaveQuestions(Game game) => Repository.StoreGameData(game);
        public Task<Game> LoadQuestions(string name) => Repository.LoadGameData(name);
        public Task<IEnumerable<GameMetadata>> LoadGames() => Repository.LoadGames();

        public Task<Question> GetCurrentQuestion(string gameName) => Repository.GetCurrentQuestion(gameName);

        public Task<GameMetadata> SetCurrentQuestion(GameMetadata game) => Repository.SetCurrentQuestion(game);

    }
}