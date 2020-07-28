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

        public async Task<Question> SetCurrentQuestion(GameMetadata game)
        {
            await Repository.SetCurrentQuestion(game);
            return await Repository.GetCurrentQuestion(game.GameId.ToString());
        }
        public async Task<GameMetadata> SetShowResults(GameMetadata game)
        {
            await Repository.UpdateGameMetadata(game);
            return game;
        }


        public async Task<GameMetadata> SetGameState(GameMetadata game)
        {
            await Repository.UpdateGameMetadata(game);
            return game;
        }

        public async Task<GameMetadata> GetGameState(int gameId)
        {
            var result = await Repository.GetGameState(gameId);
            return result;
        }

    }
}