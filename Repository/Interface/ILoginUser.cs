using CarsImgApi.Models;
using CarsImgApi.Models.DTO.LoginDTO;

namespace CarsImgApi.Repository.Interface
{
    public interface ILoginUser
    {
        public Task<LoginModel> login(UserSqlConnection user);

        public MessageModel logOut(LogOutModel userName);
    }
}
