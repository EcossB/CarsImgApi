using CarsImgApi.Models;
using CarsImgApi.Models.DTO;
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
        public async Task<IActionResult> Login(UserSqlConnection user)
        {
            var token = await _authService.Login(user);
            
            if (token is not null)
            {
                var loginResponse = new loginResponseDto
                {
                    UsuarioOracle = token.userName,
                    Token = token.token
                };

                return Ok(loginResponse);
            }
            
            ModelState.AddModelError("Error","Nombre de Usuario O Contraseña Invalidos");
            return ValidationProblem(ModelState);
        }

        [HttpPost("logout")]
        public ActionResult<IActionResult> LogOut(LogOutModel userName)
        {
            var message = _authService.LogOut(userName);
            return Ok(message);
        }




    }
}
