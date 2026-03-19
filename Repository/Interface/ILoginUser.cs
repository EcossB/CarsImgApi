using CarsImgApi.Models;
using CarsImgApi.Models.DTO.LoginDTO;

namespace CarsImgApi.Repository.Interface
{
    public interface ILoginUser
    {
        public Task<LoginModel?> Login(UserSqlConnection user);

        public MessageModel LogOut(LogOutModel userName);
    }
}
