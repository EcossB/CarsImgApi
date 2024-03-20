using System.Linq;

namespace CarsImgApi.Entity
{
    public class PoolSqlConnections
    {

        private List<UserSqlConnection> userConnections { get; set; }  = new List<UserSqlConnection>();

        public UserSqlConnection add(UserSqlConnection _user)
        {
            if ( has(_user.userName) )
            {
                this.userConnections.Remove(this.userConnections.First(u => u.Equals(_user)));   
                
            } 
            this.userConnections?.Add(_user);
            return _user;
        }

        public UserSqlConnection? get(string _username)
        {
            return this.userConnections.FirstOrDefault(u => u.userName.Equals(_username));
           
        }

        public bool remove(UserSqlConnection _user)
        {
            if( has(_user.userName) )
            {
                return this.userConnections.Remove(_user);
            }
            else
            {
                return false;
            }

        }

        public bool has(string _hasUserName)
        {
            return this.userConnections.Any(u => u.userName.Equals(_hasUserName));
        }

        public string getConnectionString(UserSqlConnection _user)
        {
            return $"User Id={_user.userName}; Password={_user.password}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = xe)))";
        }

    }
}
