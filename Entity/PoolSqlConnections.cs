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
                this.userConnections.Remove(this.userConnections.First(u => u.userName == _user.userName));   
                
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
                Console.WriteLine(userConnections.Count);
                return this.userConnections.Remove(this.userConnections.First(u => u.userName == _user.userName));
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
            return $"User Id={_user.userName.ToUpper()}; Password={_user.password.ToUpper()}; Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA =(SERVICE_NAME = xe)))";
        }

    }
}
