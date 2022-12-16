namespace OccamsRazor.Repositories
{
    using OccamsRazor.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IGameRepository
    {
        public Task<Question> GetQuestion(string gameId, uint questionNumber);
        public Task AddAnswers(IEnumerable<PlayerAnswer> answers);
    }
}