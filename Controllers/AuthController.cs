using CarsImgApi.Models;
using CarsImgApi.Models.DTO.LoginDTO;
using CarsImgApi.Repository.Interface;
using CarsImgApi.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsImgApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUser _authService;

        public AuthController(ILoginUser service)
        {
            _authService = service;
        }

        [HttpPost]
        public async Task<ActionResult<LoginModel>> login(UserSqlConnection user)
        {
            var token = await _authService.login(user);
            if (token is not null)
            {
                return Ok(token);
            }
            return BadRequest(token);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<MessageModel>> LogOut(LogOutModel userName)
        {
            var message =  _authService.logOut(userName);
            return Ok(message);
        }




    }
}
