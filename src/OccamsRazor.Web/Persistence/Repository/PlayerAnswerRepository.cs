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

            answer.Id = reader.GetInt32(0);
            answer.GameId = reader.GetInt32(1);
            answer.Round = (RoundEnum)reader.GetInt32(2);
            answer.QuestionNumber = reader.GetInt32(3);
            answer.Player = new Player() { Name = reader.GetString(4)};
            answer.AnswerText = reader.GetString(5);
            answer.Wager = reader.GetInt32(6);
            answer.PointsAwarded = reader.GetInt32(7);

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
                              SET AnswerText = @Text, Wager = @Wager, PointsAwarded=@Points
                              WHERE GameId=@GameId AND RoundNum=@Round AND QuestionNum=@Question And PlayerName=@Name",
                    conn);

                command.Parameters.AddWithValue("@GameId", answer.GameId);
                command.Parameters.AddWithValue("@Round", answer.Round);
                command.Parameters.AddWithValue("@Question", answer.QuestionNumber);
                command.Parameters.AddWithValue("@Name", answer.Player.Name ?? "");
                command.Parameters.AddWithValue("@Text", answer.AnswerText ?? "");
                command.Parameters.AddWithValue("@Wager", answer.Wager);
                command.Parameters.AddWithValue("@Points", answer.PointsAwarded);


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
                              (GameId, RoundNum, QuestionNum, PlayerName, AnswerText, Wager, PointsAwarded)
                              VALUES(@GameID, @Round, @Question, @Name, @Text, @Wager, @Points)",
                    conn);

                command.Parameters.AddWithValue("@GameId", answer.GameId);
                command.Parameters.AddWithValue("@Round", answer.Round);
                command.Parameters.AddWithValue("@Question", answer.QuestionNumber);
                command.Parameters.AddWithValue("@Name", answer.Player.Name ?? "");
                command.Parameters.AddWithValue("@Text", answer.AnswerText ?? "");
                command.Parameters.AddWithValue("@Wager", answer.Wager);
                command.Parameters.AddWithValue("@Points", 0);


                var reader = await command.ExecuteReaderAsync();
                await reader.CloseAsync();
            }

        }

        public async Task<IEnumerable<PlayerAnswer>> GetScoredAnswersForPlayerAsync(int gameId, string name)
        {
            var answers = new List<PlayerAnswer>();
            using (var conn = Context.GetSqlConnection())
            {

                var command = new SqlCommand(
                    @"SELECT a.*, g.* FROM [dbo].[PlayerAnswers] a, [dbo].[GameMetadata] g
                    WHERE a.GameId=@ID AND g.GameId=@ID AND a.PlayerName=@Name AND (a.RoundNum<g.CurrentRoundNum OR ( a.roundNum = g.CurrentRoundNum AND a.QuestionNum<g.CurrentQuestionNum))
                    ORDER BY RoundNum, QuestionNum",
                    conn);
                command.Parameters.AddWithValue("@Id", gameId);
                command.Parameters.AddWithValue("@Name", name);


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
        public async Task<IEnumerable<PlayerAnswer>> GetScoresForPlayer(int gameId, string name)
        {
            return await GetScoredAnswersForPlayerAsync(gameId, name);
        }
        public async Task<bool> DeletePlayerAnswer(PlayerAnswer answer){
            using (var conn = Context.GetSqlConnection())
            {

                await conn.OpenAsync();
                var command = new SqlCommand(
                    @"DELETE FROM [dbo].[PlayerAnswers]
                              WHERE GameId=@GameId AND RoundNum=@Round AND QuestionNum=@Question And PlayerName=@Name",
                    conn);

                command.Parameters.AddWithValue("@GameId", answer.GameId);
                command.Parameters.AddWithValue("@Round", answer.Round);
                command.Parameters.AddWithValue("@Question", answer.QuestionNumber);
                command.Parameters.AddWithValue("@Name", answer.Player.Name ?? "");


                var reader = await command.ExecuteReaderAsync();
                await reader.CloseAsync();
            } 
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