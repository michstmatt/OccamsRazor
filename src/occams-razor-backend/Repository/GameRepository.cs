namespace OccamsRazor.Repositories
{

    using OccamsRazor.Context;
    using OccamsRazor.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GameRepository : IGameRepository
    {
        private readonly OccamsRazorEfSqlContext dbContext;
        public GameRepository(OccamsRazorEfSqlContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Question> GetQuestion(string gameId, uint questionId)
        {
            return await dbContext.Questions.FindAsync(new {gameId, questionId});
        }

        public Task AddAnswers(IEnumerable<PlayerAnswer> answers)
        {
            return dbContext.Answers.AddRangeAsync(answers);
        }
    }
}