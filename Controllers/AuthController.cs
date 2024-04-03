using CarsImgApi.Entity;
using CarsImgApi.Interface;
using CarsImgApi.Models;
using CarsImgApi.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarsImgApi.Controllers
{
    [Route("api/[controller]")]
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
            var token = _authService.login(user);
            if (token.token.Length > 40)
            {
                return Ok(token);
            }
            return BadRequest(token);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<MessageModel>> LogOut(UserSqlConnection token)
        {
            var message = _authService.logOut(token);
            return Ok(message);
        }




    }
}
