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
        private readonly IGameDataRepository gameDataRepository;
        public GameDataService(IGameDataRepository repository)
        {
            gameDataRepository = repository;
        }

        public async Task<GameMetadata> SetQuestionsAsync(Game game)
        {
            var existing = await gameDataRepository.GetGameMetadataAsync(game.Metadata.GameId);
            if (existing == null)
            {
                await gameDataRepository.CreateGameMetadataAsync(game.Metadata);
                await gameDataRepository.CreateQuestionsAsync(game.Metadata.GameId, game.Questions);
            }
            else
            {
                await gameDataRepository.UpdateGameMetadataAsync(game.Metadata);
                await gameDataRepository.UpdateQuestionsAsync(game.Metadata.GameId, game.Questions);
            }

            return game.Metadata;
        }
        
        public async Task<Game> GetQuestionsAsync(int gameId) 
        {
            var game = new Game();

            game.Metadata = await gameDataRepository.GetGameMetadataAsync(gameId);
            var questions = await gameDataRepository.GetQuestionsAsync(gameId);
            game.Questions = questions.ToList();
            return game;
        } 
        public Task<ICollection<GameMetadata>> GetGamesAsync() => gameDataRepository.GetGamesAsync();

        public Task<Question> GetCurrentQuestionAsync(int gameId) => gameDataRepository.GetCurrentQuestionAsync(gameId);

        public async Task<Question> SetCurrentQuestionAsync(GameMetadata game)
        {
            await gameDataRepository.UpdateGameMetadataAsync(game);
            return await gameDataRepository.GetCurrentQuestionAsync(game.GameId);
        }

        public async Task<GameMetadata> SetGameStateAsync(GameMetadata game)
        {
            await gameDataRepository.UpdateGameMetadataAsync(game);
            return game;
        }

        public async Task<GameMetadata> GetGameStateAsync(int gameId)
        {
            var result = await gameDataRepository.GetGameMetadataAsync(gameId);
            return result;
        }

        public async Task<bool> DeleteGameAsync(int gameId) => await gameDataRepository.DeleteGameAsync(gameId);

    }
}