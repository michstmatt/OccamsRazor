using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Context;

namespace OccamsRazor.Web.Persistence.Repository
{
    public class PlayerTestAnswerRepository : IPlayerAnswerRepository
    {

        private List<PlayerAnswer> GameAnswers;

        public PlayerTestAnswerRepository()
        {
            GameAnswers = new List<PlayerAnswer>();
        }


        private PlayerAnswer FindExisistingAnswer(PlayerAnswer answer)
        {
            return GameAnswers
                .Where(a => a.Player.Name == answer.Player.Name && a.QuestionNumber == answer.QuestionNumber && a.Round == answer.Round)
                .FirstOrDefault();
        }

        public async Task<IEnumerable<PlayerAnswer>> GetScoresForPlayer(int gameId, string name)
        {
            return GameAnswers.Where(a => a.AnswerText == name);
        }


        public async Task<bool> DeletePlayerAnswer(PlayerAnswer answer)
        {
            var existingAnswer = FindExisistingAnswer(answer);
            if (existingAnswer != null)
                GameAnswers.Remove(existingAnswer);
            return true;
        }
        public async Task<bool> SubmitAnswer(PlayerAnswer answer)
        {
            var existingAnswer = FindExisistingAnswer(answer);
            
            if (existingAnswer == null)
            {
                GameAnswers.Add(answer);
            }
            else
            {
                existingAnswer.AnswerText = answer.AnswerText;
            }
            return true;
        }

        public async Task<IEnumerable<PlayerAnswer>> GetAllAnswers(int gameId)
        {
            return GameAnswers;
        }

        public async Task<bool> SubmitPlayerScores(IEnumerable<PlayerAnswer> scoredAnswers)
        {
            this.GameAnswers = scoredAnswers.ToList();
            return true;
        }

    }
}