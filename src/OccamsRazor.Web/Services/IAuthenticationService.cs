using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Service
{
    public interface IAuthenticationService
    {
        Task<bool> AddAuthentication(int gameId, string key);
        Task<bool> IsAuthenticated(int gameId, string key);
        Task<string> GenerateJwtToken(int gameId, string id);
    }
}