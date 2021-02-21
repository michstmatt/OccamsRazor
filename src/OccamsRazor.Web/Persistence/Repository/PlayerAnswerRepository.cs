using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;
using OccamsRazor.Common.Context;

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
            var answers = Context.Answers.Where(a => a.GameId == gameId).ToList();
            return answers;
        }

        public async Task<IEnumerable<PlayerAnswer>> GetAnswersForQuestion(int gameId, RoundEnum round, int questionNum)
        {
            var answers = Context.Answers.Where(a => a.GameId == gameId && a.Round == round && a.QuestionNumber == questionNum);
            return answers;
        }
        public async Task<PlayerAnswer> GetAnswersForQuestionForPlayer(PlayerAnswer answer)
        {
            var existing = Context.Answers.Where(a => a.Player.Name == answer.Player.Name && a.GameId == answer.GameId && a.QuestionNumber == answer.QuestionNumber && a.Round == answer.Round).FirstOrDefault();
            return existing;
        }

        public async Task UpdateExistingAnswerAsync(PlayerAnswer exsiting, PlayerAnswer answer)
        {
            exsiting.Wager = answer.Wager;
            exsiting.AnswerText = answer.AnswerText;
            exsiting.PointsAwarded = answer.PointsAwarded;
            await Context.SaveChangesAsync();

        }
        public async Task InsertAnswerAsync(PlayerAnswer answer)
        {
            await Context.Answers.AddAsync(answer);
            await Context.SaveChangesAsync();
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
            if (existingAnswer?.AnswerText == null)
            {
                await InsertAnswerAsync(answer);
            }
            else
            {
                await UpdateExistingAnswerAsync(existingAnswer, answer);
            }
            return true;
        }

        public async Task<IEnumerable<PlayerAnswer>> GetAllAnswers(int gameId)
        {
            return await GetAnswersForGame(gameId);
        }

        public async Task<bool> SubmitPlayerScores(IEnumerable<PlayerAnswer> scoredAnswers)
        {
            foreach (var answer in scoredAnswers)
            {
                await SubmitAnswer(answer);
            }
            return true;
        }

    }
}