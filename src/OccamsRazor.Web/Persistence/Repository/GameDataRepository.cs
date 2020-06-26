using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Context;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Persistence.Repository
{
    public class GameDataRepository : IGameDataRepository
    {
        private OccamsRazorSqlClient Context;
        public GameDataRepository(OccamsRazorSqlClient context)
        {
            Context = context;
        }


        public GameMetadata ReaderToGameMetadata(SqlDataReader reader)
        {
            var game = new GameMetadata();
            game.GameId = System.Convert.ToInt32(reader[0]);
            game.Name = System.Convert.ToString(reader[1]);
            game.CurrentRound = (RoundEnum)System.Convert.ToInt32(reader[2]);
            game.CurrentQuestion = System.Convert.ToInt32(reader[3]);
            game.ShowResults= System.Convert.ToInt32(reader[4]??0) == 1;
            return game;
        }

        public Question ReaderToQuestion(SqlDataReader reader)
        {
            var question = new Question();
            question.GameId = System.Convert.ToInt32(reader[0]);
            question.Round = (RoundEnum)System.Convert.ToInt32(reader[1]);
            question.Number = System.Convert.ToInt32(reader[2]);
            question.Text = System.Convert.ToString(reader[3]);
            question.Category = System.Convert.ToString(reader[4]);
            question.AnswerText = System.Convert.ToString(reader[5]??"");
            return question;
        }



        public async Task<IEnumerable<GameMetadata>> GetExistingGamesAsync()
        {
            var games = new List<GameMetadata>();
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"SELECT * FROM [dbo].[GameMetadata]",
                    conn);


                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    games.Add(ReaderToGameMetadata(reader));
                }
                await reader.CloseAsync();
            }
            return games;
        }

        public async Task<bool> UpdateExistingGameMetadataAsync(GameMetadata game)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"UPDATE [dbo].[GameMetadata]
                              SET Name=@Name, CurrentRoundNum=@Round, CurrentQuestionNum=@Question, ShowResults=@Show
                              WHERE GameId=@Id",
                    conn);

                command.Parameters.AddWithValue("@Name", game.Name);
                command.Parameters.AddWithValue("@Round", game.CurrentRound);
                command.Parameters.AddWithValue("@Question", game.CurrentQuestion);
                command.Parameters.AddWithValue("@Id", game.GameId);
                command.Parameters.AddWithValue("@Show", game.ShowResults?1:0);

                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                await reader.CloseAsync();
            }
            return true;
        }
        public async Task<GameMetadata> InsertGameMetadataAsync(GameMetadata game)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"INSERT INTO [dbo].[GameMetadata]
                        (Name, CurrentRoundNum, CurrentQuestionNum, ShowResults)
                        OUTPUT INSERTED.GameId
                        VALUES (@Name, @Round, @Question, @Show)",

                    conn);

                command.Parameters.AddWithValue("@Name", game.Name);
                command.Parameters.AddWithValue("@Round", game.CurrentRound);
                command.Parameters.AddWithValue("@Question", game.CurrentQuestion);
                command.Parameters.AddWithValue("@Show", game.ShowResults ? 1:0);

                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();
                game.GameId = System.Convert.ToInt32(reader[0]);
                await reader.CloseAsync();
            }
            return game;
        }

        public async Task<GameMetadata> GetGameMetadataAsync(int gameId)
        {
            GameMetadata game = null;
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"SELECT * FROM [dbo].[GameMetadata] WHERE GameId=@Id",
                    conn);

                command.Parameters.AddWithValue("@Id", gameId);
                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    game = ReaderToGameMetadata(reader);
                }
                await reader.CloseAsync();
            }
            return game;
        }
        public async Task<IEnumerable<Question>> GetQuestionsForGameAsync(int gameId)
        {
            var questions = new List<Question>();
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"SELECT * FROM [dbo].[Questions] WHERE GameId=@ID",
                    conn);
                command.Parameters.AddWithValue("@Id", gameId);


                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    questions.Add(ReaderToQuestion(reader));
                }
                await reader.CloseAsync();
            }
            return questions;
        }

        public async Task<Question> GetCurrentQuestionAsync(int gameId)
        {
            var question = new Question();
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"SELECT q.*, g.* FROM [dbo].[Questions] q, [dbo].[GameMetadata] g
                    WHERE q.GameId=@ID AND g.GameId=@ID AND q.RoundNum=g.CurrentRoundNum AND q.QuestionNum=g.CurrentQuestionNum",
                    conn);
                command.Parameters.AddWithValue("@Id", gameId);


                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    question = ReaderToQuestion(reader);
                }
                await reader.CloseAsync();
            }
            return question;
        }

        public async Task UpdateExistingQuestionsAsync(int gameId, IEnumerable<Question> questions)
        {
            using (var conn = Context.GetSqlConnection())
            {

                await conn.OpenAsync();
                foreach (var question in questions)
                {

                    var command = new SqlCommand(
                        @"UPDATE [dbo].[Questions]
                              SET QuestionText=@Text, CategoryText=@Category, AnswerText=@Answer
                              WHERE GameId=@GameId AND RoundNum=@Round AND QuestionNum=@Question",
                        conn);

                    command.Parameters.AddWithValue("@GameId", gameId);
                    command.Parameters.AddWithValue("@Round", question.Round);
                    command.Parameters.AddWithValue("@Question", question.Number);
                    command.Parameters.AddWithValue("@Text", question.Text);
                    command.Parameters.AddWithValue("@Category", question.Category);
                    command.Parameters.AddWithValue("@Answer", question.AnswerText);


                    var reader = await command.ExecuteReaderAsync();
                    await reader.CloseAsync();
                }
            }
        }
        public async Task InsertQuestionsAsync(int gameId, IEnumerable<Question> questions)
        {
            using (var conn = Context.GetSqlConnection())
            {

                await conn.OpenAsync();
                foreach (var question in questions)
                {

                    var command = new SqlCommand(
                        @"INSERT INTO [dbo].[Questions]
                              (GameId, RoundNum, QuestionNum, QuestionText, CategoryText, AnswerText)
                              VALUES(@GameID, @Round, @Question, @Text, @Category, @Answer)",
                        conn);

                    command.Parameters.AddWithValue("@GameId", gameId);
                    command.Parameters.AddWithValue("@Round", question.Round);
                    command.Parameters.AddWithValue("@Question", question.Number);
                    command.Parameters.AddWithValue("@Text", question.Text ?? "");
                    command.Parameters.AddWithValue("@Category", question.Category ?? "");
                    command.Parameters.AddWithValue("@Answer", question.AnswerText ?? "");


                    var reader = await command.ExecuteReaderAsync();
                    await reader.CloseAsync();
                }
            }
        }


        public async Task<GameMetadata> StoreGameData(Game game)
        {
            var existing = await GetExistingGamesAsync();
            if (existing.Where(g => g.GameId == game.Metadata.GameId).Any())
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
            using (var conn = Context.GetSqlConnection())
            {

                await conn.OpenAsync();


                var command = new SqlCommand(
                    @"UPDATE [dbo].[GameMetadata]
                              SET CurrentRoundNum=@Round, CurrentQuestionNum=@Question
                              WHERE GameId=@GameId",
                    conn);

                command.Parameters.AddWithValue("@Question", game.CurrentQuestion);
                command.Parameters.AddWithValue("@Round", game.CurrentRound);
                command.Parameters.AddWithValue("@GameId", game.GameId);
                var reader = await command.ExecuteReaderAsync();
                await reader.CloseAsync();
            }
            return game;
        }




        public async Task<Game> LoadGameData(string id)
        {
            int gId = int.Parse(id);
            var game = new Game();
            game.Metadata = await GetGameMetadataAsync(gId);
            var questions = await GetQuestionsForGameAsync(gId);
            game.Questions = questions.ToList();
            return game;
        }

        public async Task<IEnumerable<GameMetadata>> LoadGames()
        {
            return await GetExistingGamesAsync();
        }

        public async Task<Question> GetCurrentQuestion(string id)
        {
            return await GetCurrentQuestionAsync(int.Parse(id));
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