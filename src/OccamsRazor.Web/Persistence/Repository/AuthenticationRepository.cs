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
    public class AuthenticationRepository: IAuthenticationRepository
    {
        private OccamsRazorEfSqlContext Context;
        public AuthenticationRepository(OccamsRazorEfSqlContext context)
        {
            Context = context;
        }

        public async Task<bool> IsAuthenticatedAsync(int gameId, string key)
        {
            bool authenticated = await Context.Keys.Where(g => g.GameId == gameId && g.GameKey == key).AnyAsync();
            return authenticated;
        }

        public async Task<bool> SetAuthenticationAsync(int gameId, string key)
        {
            var exsiting = await Context.Keys.FindAsync(gameId);

            if (exsiting == null)
            {
                await Context.Keys.AddAsync(new AuthenticationModels.AuthenticationModel{GameId = gameId, GameKey = key});
            }
            else
            {
                exsiting.GameKey = key;
            }
            await Context.SaveChangesAsync();
            return true;
        }

    }
}