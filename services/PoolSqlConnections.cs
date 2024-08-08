using CarsImgApi.Models.DTO.LoginDTO;
using System.Linq;

namespace CarsImgApi.services
{
    public class PoolSqlConnections
    {

        private List<UserSqlConnection> userConnections { get; set; } = new List<UserSqlConnection>();

        public UserSqlConnection add(UserSqlConnection _user)
        {
            if (has(_user.userName))
            {
                userConnections.Remove(userConnections.First(u => u.userName == _user.userName));

            }
            userConnections?.Add(_user);
            return _user;
        }

        public UserSqlConnection? get(string _username)
        {
            return userConnections.FirstOrDefault(u => u.userName.Equals(_username));

        }

        public bool remove(LogOutModel _userName)
        {
            if (has(_userName.userName))
            {
                Console.WriteLine(userConnections.Count);
                return userConnections.Remove(userConnections.First(u => u.userName == _userName.userName));
            }
            else
            {
                return false;
            }

        }

        public bool has(string _hasUserName)
        {
            return userConnections.Any(u => u.userName.Equals(_hasUserName));
        }

        public string getConnectionString(UserSqlConnection _user)
        {
            //return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = localhost)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = pdb2)))";
            return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = xe)))";
            //return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 10.0.0.240)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = autodata)))";
        }

    }
}
