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
    public class GameTestDataRepository : IGameDataRepository
    {

        private Game GameData {get; set;}

        public GameTestDataRepository()
        {
            GameData = new Game();
            GameData.Metadata.Name = "Test";
            foreach(var q in GameData.Questions)
            {
                q.Category = "Test Category";
                q.Text = $"Round {q.Round} Question {q.Number}";
            }
            
        }

        public async Task<Game> GetGameDataAsync(int id)
        {
            return GameData;
        }
        public async Task<bool> DeleteGameAsync(int gameId)
        {
            return true;
        }

        public async Task<ICollection<GameMetadata>> GetGamesAsync()
        {
            return new GameMetadata [] {GameData.Metadata};
        }
        public async Task<ICollection<Question>> GetQuestionsAsync(int gameId)
        {
            return GameData.Questions;
        }

        public async Task<ICollection<Question>> CreateQuestionsAsync(int gameId, IList<Question> questions)
        {
            GameData.Questions = questions.ToList();
            return GameData.Questions;
        }
        public async Task<ICollection<Question>> UpdateQuestionsAsync(int gameId, IList<Question> questions)
        {
            GameData.Questions = questions.ToList();
            return GameData.Questions;
        }

        public async Task<Question> GetCurrentQuestionAsync(int id)
        {
            return GameData.Questions.Where(q => q.Number == GameData.Metadata.CurrentQuestion && q.Round == GameData.Metadata.CurrentRound).FirstOrDefault();
        }

        public async Task<GameMetadata> SetCurrentQuestionAsync(GameMetadata game)
        {
            GameData.Metadata = game;
            return game;
        }
        public async Task<GameMetadata> CreateGameMetadataAsync(GameMetadata game)
        {
            GameData.Metadata = game;
            return GameData.Metadata;
        }
        public async Task<GameMetadata> GetGameMetadataAsync(int gameId)
        {
            return GameData.Metadata;
        }
        public async Task<GameMetadata> UpdateGameMetadataAsync(GameMetadata game)
        {
            GameData.Metadata = game;
            return game;
        }

        public async Task<GameMetadata> SetGameDataAsync(Game game)
        {
            GameData = game;
            return GameData.Metadata;
        }

        public async Task<GameMetadata> SetGameStateAsync(GameMetadata game)
        {
            GameData.Metadata.State = game.State;
            return GameData.Metadata;
        }

        public async Task<GameMetadata> GetGameStateAsync(int gameId)
        {
            return GameData.Metadata;
        }
    }
}