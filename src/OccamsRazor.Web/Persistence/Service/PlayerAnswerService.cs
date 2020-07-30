using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Service;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Persistence.Service
{
    public class PlayerAnswerService : IPlayerAnswerService
    {
        public IPlayerAnswerRepository playerAnswerRepository;
        public PlayerAnswerService(IPlayerAnswerRepository repository)
        {
            playerAnswerRepository = repository;
        }

        public Task<bool> SubmitPlayerAnswer(PlayerAnswer answer)
            => playerAnswerRepository.SubmitAnswer(answer);

        public Task<IEnumerable<PlayerAnswer>> GetAllAnswers(int gameId) => playerAnswerRepository.GetAllAnswers(gameId);
        public async Task<IEnumerable<GameResults>> GetScoresForGame(int gameId)
        {
            var answers = await playerAnswerRepository.GetAllAnswers(gameId);
            var grouped = answers.GroupBy(a => a.Player.Name).Select(g => new GameResults() {
                Player = new Player(){Name = g.Key},
                PlayerAnswers = g.ToList(),
                TotalScore = g.Select(a => a?.PointsAwarded ?? 0).Sum()
            }).OrderByDescending(p => p.TotalScore);
            return grouped;
        }

        public Task<bool> SubmitPlayerScores(IEnumerable<PlayerAnswer> playerAnswers) => playerAnswerRepository.SubmitPlayerScores(playerAnswers);
        public Task<IEnumerable<PlayerAnswer>> GetScoresForPlayer(int gameId, string name) => playerAnswerRepository.GetScoresForPlayer(gameId, name);
        public Task<bool> DeletePlayerAnswer(PlayerAnswer answer) => playerAnswerRepository.DeletePlayerAnswer(answer);
    }
}