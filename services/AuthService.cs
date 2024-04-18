using CarsImgApi.Entity;
using CarsImgApi.Interface;
using CarsImgApi.Models;
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


        public LoginModel login(UserSqlConnection user)
        {
            try
            {

                var conString = BaseService._poolSqlConnections.getConnectionString(user);
                using(OracleConnection con = new OracleConnection(conString))
                {
                    con.Open();
                }
                BaseService._poolSqlConnections.add(user);

                LoginModel model = new LoginModel
                {
                    userName = user.userName,
                    password = user.password,
                    token = generateToken(user)
                };

                return model;

            } catch (Exception ex)
            {
                LoginModel model = new LoginModel
                {
                    userName = "",
                    password = "",
                    token = "Login Invalido. Compruebe Credenciales."
                };

                return model;

            }

            
        }

        public MessageModel logOut(string userName)
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

        public string generateToken(UserSqlConnection user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.userName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

    }
}
