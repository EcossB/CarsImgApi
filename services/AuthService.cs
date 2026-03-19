using CarsImgApi.Models;
using CarsImgApi.Models.DTO.LoginDTO;
using CarsImgApi.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarsImgApi.services
{
    public class AuthService : ILoginUser
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LoginModel?> Login(UserSqlConnection user)
        {
            try
            {
                var baseConnectionString = _configuration.GetConnectionString("OracleDbLogin")
                    ?? throw new InvalidOperationException("OracleDbLogin connection string is not configured.");

                var builder = new OracleConnectionStringBuilder(baseConnectionString)
                {
                    UserID = user.UserName,
                    Password = user.Password
                };

                await using (var con = new OracleConnection(builder.ConnectionString))
                {
                    await con.OpenAsync();
                }

                return new LoginModel
                {
                    UserName = user.UserName,
                    Token = GenerateToken(user)
                };
            }
            catch (OracleException ex) when (ex.Number == 1017)
            {
                return null;
            }
        }

        public MessageModel LogOut(LogOutModel userName)
        {
            return new MessageModel { Message = "Sección Cerrada Con Exito!" };
        }

        public string GenerateToken(UserSqlConnection user)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key is not configured.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(55),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
