using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Repository
{
    public interface IGameDataRepository
    {
        Task<GameMetadata> StoreGameData(Game game);
        Task<Game> LoadGameData(int gameId);
        Task<IEnumerable<GameMetadata>> LoadGames();
        Task<Question> GetCurrentQuestion(int gameId);
        Task<GameMetadata> SetCurrentQuestion(GameMetadata game);
        Task<bool> UpdateGameMetadata(GameMetadata game);
        Task<GameMetadata> SetGameState(GameMetadata game);
        Task<GameMetadata> GetGameState(int gameId);
        Task<bool> DeleteGame(int gameId);
    }
}