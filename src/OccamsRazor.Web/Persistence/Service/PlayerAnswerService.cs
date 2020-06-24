using System;
using System.Collections.Generic;
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

        public Task<bool> SubmitPlayerScores(IEnumerable<PlayerAnswer> playerAnswers) => playerAnswerRepository.SubmitPlayerScores(playerAnswers);
    }
}