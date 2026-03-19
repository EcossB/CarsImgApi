using System.ComponentModel.DataAnnotations;

namespace CarsImgApi.Models.DTO.LoginDTO
{
    public class UserSqlConnection
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
