using CarsImgApi.Models.DTO.LoginDTO;

namespace CarsImgApi.services
{
    public class BaseService
    {
        public static PoolSqlConnections _poolSqlConnections { get; set; } = new PoolSqlConnections();


        public string getConnectionString(UserSqlConnection _user)
        {
            // return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = localhost)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = pdb2)))";
            //return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = xe)))";
            //return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 10.0.0.240)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = autodata)))";
            return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = localhost)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = ORCLPDB1)))";
        }

    }
}
