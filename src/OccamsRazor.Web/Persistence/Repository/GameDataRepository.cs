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
        private OccamsRazorSqlClient Context;
        public GameDataRepository(OccamsRazorSqlClient context)
        {
            Context = context;
        }

        private GameMetadata ReaderToGameMetadata(DbDataReader reader)
        {
            var game = new GameMetadata();
            game.GameId = System.Convert.ToInt32(reader[0]);
            game.Name = System.Convert.ToString(reader[1]);
            game.CurrentRound = (RoundEnum)System.Convert.ToInt32(reader[2]);
            game.CurrentQuestion = System.Convert.ToInt32(reader[3]);
            game.State = (GameStateEnum)(System.Convert.ToInt32(reader[4] ?? 0));
            return game;
        }

        private Question ReaderToQuestion(DbDataReader reader)
        {
            var question = new Question();
            question.GameId = System.Convert.ToInt32(reader[0]);
            question.Round = (RoundEnum)System.Convert.ToInt32(reader[1]);
            question.Number = System.Convert.ToInt32(reader[2]);
            question.Text = System.Convert.ToString(reader[3]);
            question.Category = System.Convert.ToString(reader[4]);
            question.AnswerText = System.Convert.ToString(reader[5] ?? "");
            return question;
        }
        public async Task<ICollection<GameMetadata>> GetGamesAsync()
        {
            var games = new List<GameMetadata>();
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"SELECT * FROM [dbo].[GameMetadata]";


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

        public async Task<GameMetadata> UpdateGameMetadataAsync(GameMetadata game)
        {
            using (var conn = Context.GetSqlConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText =
                    @"UPDATE [dbo].[GameMetadata]
                              SET Name=@Name, CurrentRoundNum=@Round, CurrentQuestionNum=@Question, State=@State
                              WHERE GameId=@Id";

                command.Parameters.AddWithValue("@Name", game.Name);
                command.Parameters.AddWithValue("@Round", game.CurrentRound);
                command.Parameters.AddWithValue("@Question", game.CurrentQuestion);
                command.Parameters.AddWithValue("@Id", game.GameId);
                command.Parameters.AddWithValue("@State", game.State);

                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                await reader.CloseAsync();
            }
            return game;
        }
        public async Task<GameMetadata> CreateGameMetadataAsync(GameMetadata game)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[GameMetadata]
                        (Name, CurrentRoundNum, CurrentQuestionNum, State)
                        OUTPUT INSERTED.GameId
                        VALUES (@Name, @Round, @Question, @State)";


                command.Parameters.AddWithValue("@Name", game.Name);
                command.Parameters.AddWithValue("@Round", game.CurrentRound);
                command.Parameters.AddWithValue("@Question", game.CurrentQuestion);
                command.Parameters.AddWithValue("@State", game.State);

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

                var command = conn.CreateCommand();
                command.CommandText =
                    @"SELECT * FROM [dbo].[GameMetadata] WHERE GameId=@Id";

                command.Parameters.AddWithValue("@Id", gameId);
                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                if (await reader.ReadAsync())
                {
                    game = ReaderToGameMetadata(reader);
                }
                await reader.CloseAsync();
            }
            return game;
        }
        public async Task<ICollection<Question>> GetQuestionsAsync(int gameId)
        {
            var questions = new List<Question>();
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"SELECT * FROM [dbo].[Questions] WHERE GameId=@ID";

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

                var command = conn.CreateCommand();
                command.CommandText =
                    @"SELECT q.*, g.* FROM [dbo].[Questions] q, [dbo].[GameMetadata] g
                    WHERE q.GameId=@ID AND g.GameId=@ID AND q.RoundNum=g.CurrentRoundNum AND q.QuestionNum=g.CurrentQuestionNum";

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

        public async Task<ICollection<Question>> UpdateQuestionsAsync(int gameId, IList<Question> questions)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();
                command.Parameters.AddWithValue("@GameId", gameId);
                for (int i = 0; i < questions.Count(); i++)
                {
                    command.CommandText +=
                        string.Format(@"UPDATE [dbo].[Questions] 
                              SET QuestionText=@Text{0}, CategoryText=@Category{0}, AnswerText=@Answer{0}
                              WHERE GameId=@GameId AND RoundNum=@Round{0} AND QuestionNum=@Question{0};", i);

                    command.Parameters.AddWithValue($"@Round{i}", questions[i].Round);
                    command.Parameters.AddWithValue($"@Question{i}", questions[i].Number);
                    command.Parameters.AddWithValue($"@Text{i}", questions[i].Text);
                    command.Parameters.AddWithValue($"@Category{i}", questions[i].Category);
                    command.Parameters.AddWithValue($"@Answer{i}", questions[i].AnswerText);


                }
                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
            }
            return questions;
        }
        public async Task<ICollection<Question>> CreateQuestionsAsync(int gameId, IList<Question> questions)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();
                command.Parameters.AddWithValue("@GameId", gameId);
                for (int i = 0; i < questions.Count(); i++)
                {
                    command.CommandText +=
                        string.Format(@"INSERT INTO [dbo].[Questions]
                              (GameId, RoundNum, QuestionNum, QuestionText, CategoryText, AnswerText)
                              VALUES(@GameID, @Round{0}, @Question{0}, @Text{0}, @Category{0}, @Answer{0});", i);

                    command.Parameters.AddWithValue($"@Round{i}", questions[i].Round);
                    command.Parameters.AddWithValue($"@Question{i}", questions[i].Number);
                    command.Parameters.AddWithValue($"@Text{i}", questions[i].Text);
                    command.Parameters.AddWithValue($"@Category{i}", questions[i].Category);
                    command.Parameters.AddWithValue($"@Answer{i}", questions[i].AnswerText);

                }
                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
            }
            return questions;
        }

        public async Task<GameMetadata> UpdateCurrentQuestionAsync(GameMetadata game)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();


                command.CommandText +=
                    @"UPDATE [dbo].[GameMetadata]
                              SET CurrentRoundNum=@Round, CurrentQuestionNum=@Question
                              WHERE GameId=@GameId";

                command.Parameters.AddWithValue("@Question", game.CurrentQuestion);
                command.Parameters.AddWithValue("@Round", game.CurrentRound);
                command.Parameters.AddWithValue("@GameId", game.GameId);
                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
            }
            return game;
        }

        public async Task<bool> DeleteGameAsync(int gameId)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();

                command.CommandText += @"DELETE FROM [dbo].[GameMetadata] WHERE GameId=@GameId;";
                command.CommandText += @"DELETE FROM [dbo].[Questions] WHERE GameID=@GameId;";
                command.CommandText += @"DELETE FROM [dbo].[HostKeys] WHERE GameId=@GameId;";
                command.CommandText += @"DELETE FROM [dbo].[PlayerAnswers] WHERE GameId=@GameId;";

                command.Parameters.AddWithValue("@GameId", gameId);
                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
            }
            return true;
        }

    }
}