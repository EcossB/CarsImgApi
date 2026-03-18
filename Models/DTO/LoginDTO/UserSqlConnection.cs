using Oracle.ManagedDataAccess.Client;

namespace CarsImgApi.Models.DTO.LoginDTO
{
    public class UserSqlConnection
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
