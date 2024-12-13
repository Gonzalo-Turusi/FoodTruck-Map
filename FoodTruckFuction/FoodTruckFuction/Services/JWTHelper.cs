using FoodTruckFuction.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodTruckFuction.Services {
    public class JwtHelper : IJwtHelper
    {
        private readonly IEnvironmentService _environmentService;

        public JwtHelper(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public string GenerateToken(string username, string role, int expireMinutes = 60)
        {
            string secretKey = _environmentService.GetVariable("SECRET_KEY");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("SECRET_KEY is not set.");
            }

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(secretKeyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "foodtruck-app",
                audience: "foodtruck-app",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            string secretKey = _environmentService.GetVariable("SECRET_KEY");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("SECRET_KEY is not set.");
            }

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "foodtruck-app",
                ValidAudience = "foodtruck-app",
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
    }
}
