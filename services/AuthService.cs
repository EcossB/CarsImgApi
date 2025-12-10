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
            this._configuration = configuration;
        }


        public async Task<LoginModel> login(UserSqlConnection user)
        {
            try
            {
                var conString = BaseService._poolSqlConnections.getConnectionString(user);
                await using(OracleConnection con = new OracleConnection(conString))
                {
                    await con.OpenAsync();
                    await con.CloseAsync();
                }
                BaseService._poolSqlConnections.add(user);

                LoginModel model = new LoginModel
                {
                    userName = user.userName,
                    token = GenerateToken(user)
                };
                
                return model;

            } catch (Exception)
            {
               /* LoginModel model = new LoginModel
                {
                    token = "Login Invalido. Compruebe Credenciales."
                };*/

                return null;

            }

            
        }

        public MessageModel logOut(LogOutModel userName)
        {
            var message = new MessageModel();
            if (BaseService._poolSqlConnections.remove(userName))
            {
                message.message = "Log out succesfully";
                return message;
            }
            else
                message.message = "That user it's not log in, so it can't be log out";

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
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds);
        
            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
