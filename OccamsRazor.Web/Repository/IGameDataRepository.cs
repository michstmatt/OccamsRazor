using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Repository
{
    public interface IGameDataRepository
    {
        Task<GameMetadata> StoreGameData(Game game);
        Task<Game> LoadGameData(string name);
        Task<IEnumerable<GameMetadata>> LoadGames();
        Task<Question> GetCurrentQuestion(string questionNames);
        Task<GameMetadata> SetCurrentQuestion(GameMetadata game);
        Task<bool> UpdateGameMetadata(GameMetadata game);
    }
}