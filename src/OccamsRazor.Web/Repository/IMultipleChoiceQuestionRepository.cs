using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Repository
{
    public interface IMultipleChoiceRepository
    {
        Task<IEnumerable<MultipleChoiceQuestion>> LoadQuestionsAsync(int order, int count);
        Task<MultipleChoiceQuestion> GetQuestionAsync(int order, int currentQuestion);
    }
}