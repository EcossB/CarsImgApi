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
            string secret = _configuration.GetSection("Jwt:Key").Value;
            var key = Encoding.UTF8.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                /*ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false*/
                
                AuthenticationType = "Jwt", //aqui le decimos del tipoque es
                ValidateIssuer = true, //aqui le decimos que valide quien firma el token que somos nosotros
                ValidateAudience = true, // aqui validamos a la persona que va, que en este caso es el frontend
                ValidateLifetime = true, //aqui le ponemos que valide si el token expiro por el tiempo
                ValidateIssuerSigningKey = true, // aqui que valide la llave definida en el appsettings
                ValidIssuer = _configuration.GetSection("Jwt:Issuer").Value,
                ValidAudience = _configuration.GetSection("Jwt:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(key)
                
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims.Identity.Name;
        }
    }
}
