using System;
using System.Data.SqlClient;
using System.Data;

namespace OccamsRazor.Web.Context
{
    public class OccamsRazorSqlClient
    {
        public readonly string GameMetadataTable = "dbo.GameMetadata";
        private const string CONNECTION_STRING_VAR = "CONNECTION_STRING";
        public SqlConnection GetSqlConnection() =>
            new SqlConnection(Environment.GetEnvironmentVariable(CONNECTION_STRING_VAR));

    }
}