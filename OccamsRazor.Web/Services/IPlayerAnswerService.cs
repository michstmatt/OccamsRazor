using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Service
{
    public interface IPlayerAnswerService
    {
        public Task<bool> SubmitPlayerAnswer(PlayerAnswer answer);
        public Task<IEnumerable<PlayerAnswer>> GetAllAnswers(int gameId);
        public Task<bool> SubmitPlayerScores(IEnumerable<PlayerAnswer> answers);
        public Task<IEnumerable<PlayerAnswer>> GetScoresForPlayer(int gameId, string name);
        public Task<IEnumerable<GameResults>> GetScoresForGame(int gameId);
    }
}