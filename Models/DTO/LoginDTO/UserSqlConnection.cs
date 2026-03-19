using Oracle.ManagedDataAccess.Client;

namespace CarsImgApi.Models.DTO.LoginDTO
{
    public class UserSqlConnection
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
