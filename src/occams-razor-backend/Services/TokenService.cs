namespace OccamsRazor.Services
{
    using OccamsRazor.Models;
    using Microsoft.IdentityModel.Tokens; 
    using System.IdentityModel.Tokens.Jwt;
    using System;
    using System.Security.Claims;
    using System.Linq;
    using System.Text;

    public class TokenService
    {
        private readonly SigningCredentials signingCredentials;
        private string issuer;

        private readonly TokenValidationParameters tokenParams;

        public TokenService(byte[] keyData, string issuer, TokenValidationParameters tokenParams)
        {
            var key = new SymmetricSecurityKey(keyData);
            this.signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            this.issuer = issuer;
            this.tokenParams = tokenParams;
        }

        public static Player PlayerFromClaimsPrincipal(ClaimsPrincipal user)
        {
            return new Player
            (
                user.Claims.Where(c => c.Type == Constants.ClaimPlayerName).First().Value,
                user.Claims.Where(c => c.Type == Constants.ClaimGameId).First().Value,
                Guid.Parse(user.Claims.Where(c => c.Type == Constants.ClaimPlayerId).First().Value),
                (PlayerRole)Enum.Parse(typeof(PlayerRole), user.Claims.Where(c => c.Type == Constants.ClaimRole).First().Value)
            );
        }

        public Player ValidateToken(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token;
            var result = tokenHandler.ValidateToken(jwt, tokenParams, out token);
            return PlayerFromClaimsPrincipal(result);
        }

        public string GenerateToken(Player player)
        {
            var claims = new []
            {
                new Claim(Constants.ClaimPlayerId, player.Id.ToString()),
                new Claim(Constants.ClaimPlayerName, player.Name),
                new Claim(Constants.ClaimRole, player.Role.ToString()),
                new Claim(Constants.ClaimGameId, player.GameId)
            };

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: System.DateTime.Now.AddHours(6),
                signingCredentials: this.signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    } 
}