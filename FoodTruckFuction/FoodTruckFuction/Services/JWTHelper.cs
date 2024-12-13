using FoodTruckFuction.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtHelper : IJwtHelper
{ 
    public string GenerateToken(string username, string role, int expireMinutes = 30)
    {
        string SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
        byte[] SecretKeyBytes = Encoding.UTF8.GetBytes(SecretKey);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(SecretKeyBytes);
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
        string SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
        byte[] SecretKeyBytes = Encoding.UTF8.GetBytes(SecretKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "foodtruck-app",
            ValidAudience = "foodtruck-app",
            IssuerSigningKey = new SymmetricSecurityKey(SecretKeyBytes)
        };

        return tokenHandler.ValidateToken(token, validationParameters, out _);
    }
}
