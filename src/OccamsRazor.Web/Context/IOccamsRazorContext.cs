using System.Data.Common;

namespace OccamsRazor.Web.Context 
{
    interface IOccamsRazorContext
    {
        DbConnection GetSqlConnection();
    }
}