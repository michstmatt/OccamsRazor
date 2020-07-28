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
        Task<Game> LoadQuestions(string name);
        Task<IEnumerable<GameMetadata>> LoadGames();

        Task<Question> GetCurrentQuestion(string gameName);
        Task<Question> SetCurrentQuestion(GameMetadata game);

        Task<GameMetadata> SetShowResults(GameMetadata game);

        Task<GameMetadata> SetGameState(GameMetadata game);
        Task<GameMetadata> GetGameState(int gameId);
    }
}