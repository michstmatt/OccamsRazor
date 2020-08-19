using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
namespace OccamsRazor.Web.Repository
{
    public interface IPlayerAnswerRepository
    {
        Task<bool> SetPlayerAnswerAsync(PlayerAnswer answer);
        Task<IEnumerable<PlayerAnswer>> GetAnswersAsync(int gameId);
        Task<bool> SetPlayerScoresAsync(IEnumerable<PlayerAnswer> scoredAnswers);
        Task<IEnumerable<PlayerAnswer>> GetScoresForPlayerAsync(int gameId, string name);
        Task<bool> DeletePlayerAnswerAsync(PlayerAnswer answer);
    }
}