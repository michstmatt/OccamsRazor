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
    }
}