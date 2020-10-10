using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Context;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Persistence.Repository
{
    public class GameDataRepository : IGameDataRepository
    {
        private OccamsRazorEfSqlContext Context;
        public GameDataRepository(OccamsRazorEfSqlContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<GameMetadata>> GetExistingGamesAsync()
        {
            return await Context.GameMetadata.ToListAsync();
        }

        public async Task<bool> UpdateExistingGameMetadataAsync(GameMetadata game)
        {
            var existing = await Context.GameMetadata.Where(g => g.GameId == game.GameId).FirstOrDefaultAsync();
            existing.Name = game.Name;
            existing.CurrentRound = game.CurrentRound;
            existing.CurrentQuestion = game.CurrentQuestion;
            existing.State = game.State;

            await Context.SaveChangesAsync();
            return true;
        }
        public async Task<GameMetadata> InsertGameMetadataAsync(GameMetadata game)
        {
            await Context.GameMetadata.AddAsync(game);
            await Context.SaveChangesAsync();
            return game;
        }

        public async Task<GameMetadata> GetGameMetadataAsync(int gameId)
        {
            return await Context.GameMetadata.Where(g => g.GameId == gameId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Question>> GetQuestionsForGameAsync(int gameId)
        {
            return await Context.Questions.Where(g => g.GameId == gameId).ToListAsync();
        }

        public async Task<Question> GetCurrentQuestionAsync(int gameId)
        {
            var currentGame = await Context.GameMetadata.Where(g => g.GameId == gameId).FirstOrDefaultAsync();
            var question = await Context.Questions.Where(g => g.GameId == gameId && g.Round == currentGame.CurrentRound && g.Number == currentGame.CurrentQuestion).FirstOrDefaultAsync();
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
            await Context.Questions.AddRangeAsync(questions);
            await Context.SaveChangesAsync();
        }


        public async Task<GameMetadata> StoreGameData(Game game)
        {
            var existing = await Context.GameMetadata.Where(g => g.GameId.ToString() == game.Id).FirstOrDefaultAsync();
            if (existing != null)
            {
                await UpdateExistingGameMetadataAsync(game.Metadata);
                await UpdateExistingQuestionsAsync(game.Metadata.GameId, game.Questions);
            }
            else
            {
                await InsertGameMetadataAsync(game.Metadata);
                await InsertQuestionsAsync(game.Metadata.GameId, game.Questions);
            }
            return game.Metadata;
        }


        public async Task<GameMetadata> UpdateCurrentQuestionAsync(GameMetadata game)
        {
            var currentGame = await Context.GameMetadata.Where(g => g.GameId == game.GameId).FirstOrDefaultAsync();
            currentGame.CurrentRound = game.CurrentRound;
            currentGame.CurrentQuestion = game.CurrentQuestion;

            await Context.SaveChangesAsync();
            return currentGame;
        }

        public async Task<bool> DeleteGame(int gameId)
        {
            var existingGame = await Context.GameMetadata.Where(g => g.GameId == gameId).FirstOrDefaultAsync();
            var existingQuestions = await Context.Questions.Where(g => g.GameId == gameId).ToArrayAsync();
            var exisitingAnswers = await Context.Answers.Where(g => g.GameId == gameId).ToArrayAsync();

            Context.GameMetadata.Remove(existingGame);
            Context.Questions.RemoveRange(existingQuestions);
            Context.Answers.RemoveRange(exisitingAnswers);

            return true;
        }

        public async Task<GameMetadata> SetGameState(GameMetadata game)
        {
            return await Task.FromResult(game);
        }
        public async Task<GameMetadata> GetGameState(int gameId)
        {
            return await GetGameMetadataAsync(gameId);
        }


        public async Task<Game> LoadGameData(int id)
        {
            var game = new Game();
            game.Metadata = await GetGameMetadataAsync(id);
            var questions = await GetQuestionsForGameAsync(id);
            game.Questions = questions.ToList();
            return game;
        }

        public async Task<IEnumerable<GameMetadata>> LoadGames()
        {
            return await GetExistingGamesAsync();
        }

        public async Task<Question> GetCurrentQuestion(int id)
        {
            return await GetCurrentQuestionAsync(id);
        }

        public async Task<GameMetadata> SetCurrentQuestion(GameMetadata game)
        {
            return await UpdateCurrentQuestionAsync(game);
        }
        public async Task<bool> UpdateGameMetadata(GameMetadata game)
        {
            return await UpdateExistingGameMetadataAsync(game);
        }

    }
}