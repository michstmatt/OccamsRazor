using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Repository
{
    public interface IAuthenticationRepository
    {
        Task<bool> SetAuthenticationAsync(int gameId, string key);
        Task<bool> IsAuthenticatedAsync(int gameId, string key);
    }
}