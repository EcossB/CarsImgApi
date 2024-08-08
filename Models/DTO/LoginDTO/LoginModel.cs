namespace CarsImgApi.Models.DTO.LoginDTO
{
    public class LoginModel
    {
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

        public string token { get; set; } = string.Empty;
    }
}
