using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Service
{
    public interface IGameDataService
    {
        Task<GameMetadata> SetQuestionsAsync(Game game);
        Task<Game> GetQuestionsAsync(int gameId);
        Task<ICollection<GameMetadata>> GetGamesAsync();
        Task<Question> GetCurrentQuestionAsync(int gameId);
        Task<Question> SetCurrentQuestionAsync(GameMetadata game);
        Task<GameMetadata> SetGameStateAsync(GameMetadata game);
        Task<GameMetadata> GetGameStateAsync(int gameId);
        Task<bool> DeleteGameAsync(int gameId);
    }
}