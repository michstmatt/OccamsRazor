using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
namespace OccamsRazor.Web.Repository
{
    public interface IPlayerAnswerRepository
    {
        public Task<bool> SubmitAnswer(PlayerAnswer answer);
        public Task<IEnumerable<PlayerAnswer>> GetAllAnswers(int gameId);
        public Task<bool> SubmitPlayerScores(IEnumerable<PlayerAnswer> scoredAnswers);

        public Task<IEnumerable<PlayerAnswer>> GetScoresForPlayer(int gameId, string name);
        public Task<bool> DeletePlayerAnswer(PlayerAnswer answer);
    }
}