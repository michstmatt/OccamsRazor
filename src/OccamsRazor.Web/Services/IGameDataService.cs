using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Service
{
    public interface IGameDataService
    {
        Task<GameMetadata> CreateGameAsync(Game game);
        Task<GameMetadata> SaveGameAsync(Game game);
        Task<AbstractGame> LoadGameAsync(int gameId);
        Task<IEnumerable<GameMetadata>> LoadGamesAsync();
        Task<AbstractQuestion> GetCurrentQuestionAsync(int gameId);
        Task<AbstractQuestion> SetCurrentQuestionAsync(GameMetadata game);
        Task<GameMetadata> SetGameStateAsync(GameMetadata game);
        Task<GameMetadata> GetGameStateAsync(int gameId);
        Task<bool> DeleteGameAsync(int gameId);
    }
}