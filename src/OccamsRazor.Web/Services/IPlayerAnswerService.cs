using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Service
{
    public interface IPlayerAnswerService
    {
        Task<bool> SubmitPlayerAnswerAsync(PlayerAnswer answer);
        Task<IEnumerable<PlayerAnswer>> GetAllAnswersAsync(int gameId);
        Task<bool> SubmitPlayerScoresAsync(IEnumerable<PlayerAnswer> answers);
        Task<IEnumerable<PlayerAnswer>> GetScoresForPlayerAsync(int gameId, string name);
        Task<IEnumerable<GameResults>> GetScoresForGameAsync(int gameId);
        Task<bool> DeletePlayerAnswerAsync(PlayerAnswer answer);
    }
}