using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;

using OccamsRazor.Common.Models;

using OccamsRazor.Common.Context;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Persistence.Repository
{
    public class GameDataRepository : IGameDataRepository
    {
        private OccamsRazorEfSqlContext Context;
        public GameDataRepository(OccamsRazorEfSqlContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<GameMetadata>> GetExistingGamesAsync()
        {
            return await Context.GameMetadata.Where(x => x !=null).ToListAsync();
        }

        public async Task<GameMetadata> UpdateExistingGameMetadataAsync(GameMetadata game)
        {
            var existing = await Context.GameMetadata.Where(g => g.GameId == game.GameId).FirstOrDefaultAsync();
            existing.Name = game.Name;
            existing.CurrentRound = game.CurrentRound;
            existing.CurrentQuestion = game.CurrentQuestion;
            existing.State = game.State;

            Context.SaveChanges();
            return existing;
        }
        public async Task<GameMetadata> InsertGameMetadataAsync(GameMetadata game)
        {
            await Context.GameMetadata.AddAsync(game);
            await Context.SaveChangesAsync();
            return game;
        }

        public async Task<GameMetadata> GetGameMetadataAsync(int gameId)
        {
            var existing = await Context.GameMetadata.Where(g => g.GameId == gameId).FirstOrDefaultAsync();
            return existing;
        }

        public async Task DeleteGameMetadataAsync(int gameId)
        {
            var existingGame = await Context.GameMetadata.Where(g => g.GameId == gameId).FirstOrDefaultAsync();
            Context.GameMetadata.Remove(existingGame);
            await Context.SaveChangesAsync();
        }

    }
}