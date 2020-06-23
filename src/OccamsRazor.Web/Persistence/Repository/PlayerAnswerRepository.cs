using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Context;

namespace OccamsRazor.Web.Persistence.Repository
{
    public class PlayerAnswerRepository : IPlayerAnswerRepository
    {
        private OccamsRazorSqlClient Context;
        public PlayerAnswerRepository(OccamsRazorSqlClient context)
        {
            Context = context;
        }
        public PlayerAnswer ReaderToAnswer(SqlDataReader reader)
        {
            var answer = new PlayerAnswer();

            answer.Id = System.Convert.ToInt32(reader[0]);
            answer.GameId = System.Convert.ToInt32(reader[1]);
            answer.Round = (RoundEnum)System.Convert.ToInt32(reader[2]);
            answer.QuestionNumber = System.Convert.ToInt32(reader[3]);
            answer.Player = new Player() { Name = System.Convert.ToString(reader[4]) };
            answer.AnswerText = System.Convert.ToString(reader[5]);
            answer.Wager = System.Convert.ToInt32(reader[6]);
            return answer;
        }

        public async Task<IEnumerable<PlayerAnswer>> GetAnswersForGame(int gameId)
        {
            var answers = new List<PlayerAnswer>();
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"SELECT * FROM [dbo].[PlayerAnswers]
                    WHERE GameId=@GameId
                    ORDER BY RoundNum, QuestionNum ASC",
                    conn);
                command.Parameters.AddWithValue("@GameId", gameId);

                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    answers.Add(ReaderToAnswer(reader));
                }
                await reader.CloseAsync();
            }
            return answers;
        }

        public async Task<IEnumerable<PlayerAnswer>> GetAnswersForQuestion(int gameId, RoundEnum round, int questionNum)
        {
            var answers = new List<PlayerAnswer>();
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"SELECT * FROM [dbo].[PlayerAnswers]
                    WHERE GameId=@GameId AND RoundNum=@Round AND QuestionNum=@Question",
                    conn);
                command.Parameters.AddWithValue("@GameId", gameId);
                command.Parameters.AddWithValue("@Round", round);
                command.Parameters.AddWithValue("@Question", questionNum);


                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    answers.Add(ReaderToAnswer(reader));
                }
                await reader.CloseAsync();
            }
            return answers;
        }
        public async Task<PlayerAnswer> GetAnswersForQuestionForPlayer(PlayerAnswer answer)
        {
            var existing = new PlayerAnswer();
            using (var conn = Context.GetSqlConnection())
            {
                var command = new SqlCommand(
                    @"SELECT * FROM [dbo].[PlayerAnswers]
                    WHERE GameId=@GameId AND RoundNum=@Round AND QuestionNum=@Question AND PlayerName=@Name",
                    conn);
                command.Parameters.AddWithValue("@GameId", answer.GameId);
                command.Parameters.AddWithValue("@Round", answer.Round);
                command.Parameters.AddWithValue("@Question", answer.QuestionNumber);
                command.Parameters.AddWithValue("@Name", answer.Player.Name);


                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    existing = ReaderToAnswer(reader);
                }
                await reader.CloseAsync();
            }
            return existing;
        }

        public async Task UpdateExistingAnswerAsync(PlayerAnswer answer)
        {
            using (var conn = Context.GetSqlConnection())
            {

                await conn.OpenAsync();
                var command = new SqlCommand(
                    @"UPDATE [dbo].[PlayerAnswers]
                              SET AnswerText = @Text, Wager = @Wager
                              WHERE GameId=@GameId AND RoundNum=@Round AND QuestionNum=@Question And PlayerName=@Name",
                    conn);

                command.Parameters.AddWithValue("@GameId", answer.GameId);
                command.Parameters.AddWithValue("@Round", answer.Round);
                command.Parameters.AddWithValue("@Question", answer.QuestionNumber);
                command.Parameters.AddWithValue("@Name", answer.Player.Name ?? "");
                command.Parameters.AddWithValue("@Text", answer.AnswerText ?? "");
                command.Parameters.AddWithValue("@Wager", answer.Wager);


                var reader = await command.ExecuteReaderAsync();
                await reader.CloseAsync();
            }
        }
        public async Task InsertAnswerAsync(PlayerAnswer answer)
        {
            using (var conn = Context.GetSqlConnection())
            {

                await conn.OpenAsync();

                var command = new SqlCommand(
                    @"INSERT INTO [dbo].[PlayerAnswers]
                              (GameId, RoundNum, QuestionNum, PlayerName, AnswerText, Wager)
                              VALUES(@GameID, @Round, @Question, @Name, @Text, @Wager)",
                    conn);

                command.Parameters.AddWithValue("@GameId", answer.GameId);
                command.Parameters.AddWithValue("@Round", answer.Round);
                command.Parameters.AddWithValue("@Question", answer.QuestionNumber);
                command.Parameters.AddWithValue("@Name", answer.Player.Name ?? "");
                command.Parameters.AddWithValue("@Text", answer.AnswerText ?? "");
                command.Parameters.AddWithValue("@Wager", answer.Wager);


                var reader = await command.ExecuteReaderAsync();
                await reader.CloseAsync();
            }

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

    }
}