using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Context;

namespace OccamsRazor.Web.Persistence.Repository
{
    public class PlayerAnswerRepository : IPlayerAnswerRepository
    {
        private OccamsRazorEfSqlContext Context;
        public PlayerAnswerRepository(OccamsRazorEfSqlContext context)
        {
            Context = context;
        }
        public async Task<IEnumerable<PlayerAnswer>> GetAnswersForGame(int gameId)
        {
            var answers = Context.Answers.Where(a => a.GameId == gameId);
            return answers;
        }

        public async Task<IEnumerable<PlayerAnswer>> GetAnswersForQuestion(int gameId, RoundEnum round, int questionNum)
        {
            var answers =  Context.Answers.Where(a => a.GameId == gameId && a.Round == round && a.QuestionNumber == questionNum);
            return answers;
        }
        public async Task<PlayerAnswer> GetAnswersForQuestionForPlayer(PlayerAnswer answer)
        {
            var existing = Context.Answers.Where(a => a.Player == answer.Player && a.GameId == answer.GameId && a.QuestionNumber == answer.QuestionNumber && a.Round == answer.Round).FirstOrDefault();
            return existing;
        }

        public async Task UpdateExistingAnswerAsync(PlayerAnswer answer)
        {
            var existing = Context.Answers.Where(a => a.Player == answer.Player && a.GameId == answer.GameId && a.QuestionNumber == answer.QuestionNumber && a.Round == answer.Round).FirstOrDefault();

            if (existing == null)
            {

            }
            existing.Wager = answer.Wager;
            existing.AnswerText = answer.AnswerText;
            existing.PointsAwarded = answer.PointsAwarded;
            await Context.SaveChangesAsync();

        }
        public async Task InsertAnswerAsync(PlayerAnswer answer)
        {
            await Context.Answers.AddAsync(answer);
        }

        public async Task<IEnumerable<PlayerAnswer>> GetScoredAnswersForPlayerAsync(int gameId, string name)
        {
            var answers = Context.Answers.Where(a => a.GameId == gameId && a.Player.Name == name);
            return answers;
        }
        public async Task<IEnumerable<PlayerAnswer>> GetScoresForPlayer(int gameId, string name)
        {
            return await GetScoredAnswersForPlayerAsync(gameId, name);
        }
        public async Task<bool> DeletePlayerAnswer(PlayerAnswer answer)
        {

            var existing = Context.Answers.Where(a => a.Player == answer.Player && a.GameId == answer.GameId && a.QuestionNumber == answer.QuestionNumber && a.Round == answer.Round).FirstOrDefault();
            Context.Answers.Remove(existing);
            await Context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SubmitAnswer(PlayerAnswer answer)
        {

            var existingAnswer = await GetAnswersForQuestionForPlayer(answer);
            if (existingAnswer.AnswerText == null)
            {
                await InsertAnswerAsync(answer);
            }
            else
            {
                await UpdateExistingAnswerAsync(answer);
            }
            return true;
        }

        public async Task<IEnumerable<PlayerAnswer>> GetAllAnswers(int gameId)
        {
            return await GetAnswersForGame(gameId);
        }

        public async Task<bool> SubmitPlayerScores(IEnumerable<PlayerAnswer> scoredAnswers)
        {
            foreach(var answer in scoredAnswers)
            {
                await UpdateExistingAnswerAsync(answer);
            }
            return true;
        }

    }
}