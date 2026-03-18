using CarsImgApi.Models;
using CarsImgApi.Models.DTO.LoginDTO;
using CarsImgApi.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarsImgApi.services
{
    public class AuthService : BaseService, ILoginUser
    {

        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<LoginModel> Login(UserSqlConnection user)
        {
            try
            {
                var baseConnectionString = _configuration.GetConnectionString("OracleDbLogin");

                // 2. Usas el constructor seguro
                var builder = new OracleConnectionStringBuilder(baseConnectionString)
                {
                    // El builder se encarga de escapar cualquier carácter peligroso automáticamente
                    UserID = user.userName,
                    Password = user.password
                };

                // 3. Obtienes el string final sanitizado
                string conStringSeguro = builder.ConnectionString;

                await using(OracleConnection con = new OracleConnection(conStringSeguro))
                {
                    await con.OpenAsync();
                    await con.CloseAsync();
                }

                LoginModel model = new LoginModel
                {
                    userName = user.userName,
                    token = GenerateToken(user)
                };
                
                return model;

            } catch (OracleException ex)
            {
                // ORA-01017 es el código de Oracle para "invalid username/password; logon denied"
                if (ex.Number == 1017)
                {
                    return null; // esto disparara un 401
                }

                throw;

            }

            
        }

        public MessageModel LogOut(LogOutModel userName)
        {
            var message = new MessageModel()
            {
                message = "Sección Cerrada Con Exito!"
            };

            return message;
        }

        public string GenerateToken(UserSqlConnection user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.userName)
            };

            /*var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;*/
            
            var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // creando las credenciales del token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],   
                claims: claims,
                expires: DateTime.Now.AddMinutes(55),
                signingCredentials: creds);
        
            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
