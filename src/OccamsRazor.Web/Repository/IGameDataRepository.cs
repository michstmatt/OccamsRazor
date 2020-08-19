using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Repository
{
    public interface IGameDataRepository
    {
        Task<ICollection<Question>> GetQuestionsAsync(int gameId);
        Task<ICollection<Question>> CreateQuestionsAsync(int gameId, IList<Question> questions);
        Task<ICollection<Question>> UpdateQuestionsAsync(int gameId, IList<Question> questions);
        Task<ICollection<GameMetadata>> GetGamesAsync();
        Task<Question> GetCurrentQuestionAsync(int gameId);
        Task<GameMetadata> GetGameMetadataAsync(int gameId);
        Task<GameMetadata> CreateGameMetadataAsync(GameMetadata game);
        Task<GameMetadata> UpdateGameMetadataAsync(GameMetadata game);
        Task<bool> DeleteGameAsync(int gameId);
    }
}