using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Context;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Persistence.Service
{
    public class AuthenticationService: IAuthenticationService
    {
        private IAuthenticationRepository authenticationRepository;

        public AuthenticationService(IAuthenticationRepository repo) => this.authenticationRepository = repo;
        public async Task<bool> IsAuthenticated(int gameId, string key)
        {
            return await authenticationRepository.IsAuthenticatedAsync(gameId, key);
        }

        public async Task<bool> AddAuthentication(int gameId, string key)
        {
            return await authenticationRepository.SetAuthenticationAsync(gameId, key);
        }
    }
}