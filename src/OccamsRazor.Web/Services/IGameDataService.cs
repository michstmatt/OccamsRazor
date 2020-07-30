using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Service
{
    public interface IGameDataService
    {
        Task<GameMetadata> SaveQuestions(Game game);
        Task<Game> LoadQuestions(int gameId);
        Task<IEnumerable<GameMetadata>> LoadGames();

        Task<Question> GetCurrentQuestion(int gameId);
        Task<Question> SetCurrentQuestion(GameMetadata game);

        Task<GameMetadata> SetShowResults(GameMetadata game);

        Task<GameMetadata> SetGameState(GameMetadata game);
        Task<GameMetadata> GetGameState(int gameId);
        Task<bool> DeleteGame(int gameId);
    }
}