using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;

using OccamsRazor.Common.Models;

using OccamsRazor.Common.Context;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Persistence.Repository
{
    public class MultipleChoiceQuestionRepository : IMultipleChoiceRepository
    {
        private OccamsRazorEfSqlContext Context;
        public MultipleChoiceQuestionRepository(OccamsRazorEfSqlContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<MultipleChoiceQuestion>> LoadQuestionsAsync(int order, int count)
        {
            var rand = new Random(order);
            return Context.McQuestions.AsEnumerable().OrderBy(_ => rand.Next()).Take(count);
        }

        public async Task<MultipleChoiceQuestion> GetQuestionAsync(int order, int currentQuestion)
        {
            var list = await LoadQuestionsAsync(order, currentQuestion);
            return list.LastOrDefault();
        }

    }
}