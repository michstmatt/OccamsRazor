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
    public class QuestionRepository: IQuestionRepository
    {
        private OccamsRazorEfSqlContext Context;
        public QuestionRepository(OccamsRazorEfSqlContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<Question>> LoadQuestionsAsync(int gameId)
        {
            var questions = await Context.Questions.Where(g => g!= null && g.GameId == gameId).ToListAsync();
            return questions;
        }

        public async Task<Question> GetQuestionAsync(int gameId, RoundEnum currentRound, int currentQuestion)
        {
            var question = await Context.Questions.Where(g => g.GameId == gameId && g.Round == currentRound && g.Number == currentQuestion).FirstOrDefaultAsync();
            return question;
        }

        public async Task UpdateExistingQuestionsAsync(int gameId, IList<Question> questions)
        {
            foreach(var question in questions)
            {
                var existing = await Context.Questions.Where(q => q.GameId == question.GameId && q.Round == question.Round && q.Number == question.Number).FirstOrDefaultAsync();
                existing.AnswerText = question.AnswerText;
                existing.Category = question.Category;
                existing.Text = question.Text;
            }
            await Context.SaveChangesAsync();
        }
        public async Task InsertQuestionsAsync(int gameId, IList<Question> questions)
        {
            var updatedId = questions.Select(s => { s.GameId = gameId; return s; });
            await Context.Questions.AddRangeAsync(updatedId);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteQuestionsAsync(int gameId)
        {
            var questions = await Context.Questions.Where(q => q.GameId == gameId).ToArrayAsync();
            Context.Questions.RemoveRange(questions);
            await Context.SaveChangesAsync();
        }

    }
}