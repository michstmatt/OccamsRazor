using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Repository
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> LoadQuestionsAsync (int gameId);
        Task<Question> GetQuestionAsync(int gameId, RoundEnum currentRound, int currentQuestion);
        Task UpdateExistingQuestionsAsync(int gameId, IList<Question> questions);
        Task InsertQuestionsAsync(int gameId, IList<Question> questions);
        Task DeleteQuestionsAsync(int gameId);
    }
}