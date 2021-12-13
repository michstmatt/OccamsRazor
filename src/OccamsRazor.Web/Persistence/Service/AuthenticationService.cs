using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using OccamsRazor.Common.Models;

using OccamsRazor.Web.Configuration;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Persistence.Service
{
    public class AuthenticationService: IAuthenticationService
    {
        private IAuthenticationRepository authenticationRepository;
        private IGameDataService gameDataService;
        private JwtConfiguration configuration;

        public AuthenticationService(IAuthenticationRepository repo, IGameDataService gameService, JwtConfiguration cfg)
        {
            this.authenticationRepository = repo;
            this.gameDataService = gameService;
            this.configuration = cfg;
        }
        public async Task<bool> IsAuthenticated(int gameId, string key)
        {
            return await authenticationRepository.IsAuthenticatedAsync(gameId, key);
        }

        public async Task<bool> AddAuthentication(int gameId, string key)
        {
            return await authenticationRepository.SetAuthenticationAsync(gameId, key);
        }

        public async Task<string> GenerateJwtToken(int gameId, string id)
        {
            var game = await this.gameDataService.LoadGameAsync(gameId);

            if (game == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", id), new Claim("gameId", gameId.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = configuration.Issuer,
                Audience = configuration.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}