using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Repository
{
    public interface IGameDataRepository
    {
        Task<IEnumerable<GameMetadata>> GetExistingGamesAsync();
        Task<GameMetadata> UpdateExistingGameMetadataAsync(GameMetadata game);
        Task<GameMetadata> InsertGameMetadataAsync(GameMetadata game);
        Task<GameMetadata> GetGameMetadataAsync(int gameId);
        Task DeleteGameMetadataAsync(int gameId);
    }
}