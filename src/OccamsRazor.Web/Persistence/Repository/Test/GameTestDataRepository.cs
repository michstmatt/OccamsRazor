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

        public async Task<Game> LoadGameData(string id)
        {
            return GameData;
        }

        public async Task<IEnumerable<GameMetadata>> LoadGames()
        {
            return new GameMetadata [] {GameData.Metadata};
        }

        public async Task<Question> GetCurrentQuestion(string id)
        {
            return GameData.Questions.Where(q => q.Number == GameData.Metadata.CurrentQuestion && q.Round == GameData.Metadata.CurrentRound).FirstOrDefault();
        }

        public async Task<GameMetadata> SetCurrentQuestion(GameMetadata game)
        {
            GameData.Metadata = game;
            return game;
        }
        public async Task<bool> UpdateGameMetadata(GameMetadata game)
        {
            GameData.Metadata = game;
            return true;
        }

        public async Task<GameMetadata> StoreGameData(Game game)
        {
            GameData = game;
            return GameData.Metadata;
        }

        public async Task<GameMetadata> SetGameState(GameMetadata game)
        {
            GameData.Metadata.State = game.State;
            return GameData.Metadata;
        }

        public async Task<GameMetadata> GetGameState(int gameId)
        {
            return GameData.Metadata;
        }
    }
}