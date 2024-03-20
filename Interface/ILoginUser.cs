using CarsImgApi.Entity;
using CarsImgApi.Models;

namespace CarsImgApi.Interface
{
    public interface ILoginUser
    {
        public LoginModel login(UserSqlConnection user);

        public void logOut(UserSqlConnection user);
    }
}
