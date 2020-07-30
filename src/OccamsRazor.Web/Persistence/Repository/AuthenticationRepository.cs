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
        private OccamsRazorSqlClient Context;
        public AuthenticationRepository(OccamsRazorSqlClient context)
        {
            Context = context;
        }

        public async Task<bool> IsAuthenticatedAsync(int gameId, string key)
        {
            bool authenticated = false;
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"SELECT COUNT(*) FROM [dbo].[HostKeys] 
                    WHERE GameId=@GameId AND GameKey=@GameKey";

                command.Parameters.AddWithValue("@GameId", gameId);
                command.Parameters.AddWithValue("@GameKey", key);

                await conn.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var count = System.Convert.ToInt32(reader?.GetValue(0) ?? 0);
                    authenticated = count == 1;
                }
                await reader.CloseAsync();
            }
            return authenticated;
        }

        public async Task<bool> SetAuthenticationAsync(int gameId, string key)
        {
            using (var conn = Context.GetSqlConnection())
            {

                var command = conn.CreateCommand();
                command.CommandText =
                    @"INSERT INTO [dbo].[HostKeys]
                    (GameId, GameKey) 
                    VALUES (@GameId, @GameKey)";

                command.Parameters.AddWithValue("@GameId", gameId);
                command.Parameters.AddWithValue("@GameKey", key);

                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
            }
            return true;
        }

    }
}