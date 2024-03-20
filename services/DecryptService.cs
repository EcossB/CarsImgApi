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


        /*This method take the token and extract the name. */
        public string GetName(string token)
        {
            string secret = _configuration.GetSection("AppSettings:Token").Value;
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims.Identity.Name;
        }
    }
}
