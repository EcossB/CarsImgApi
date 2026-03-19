using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CarsImgApi.services
{
    public class DecryptService
    {
        private readonly IConfiguration _configuration;

        public DecryptService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? GetName(string token)
        {
            var secret = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is not configured.");

            var key = Encoding.UTF8.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();

            var validations = new TokenValidationParameters
            {
                AuthenticationType = "Jwt",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var claims = handler.ValidateToken(token, validations, out _);
            return claims.Identity?.Name;
        }
    }
}
