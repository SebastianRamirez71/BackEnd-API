using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_TPI_PSS.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController (IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("users/createUser")]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            if (_userService.CreateUser(userDto))
            {
                return Ok($"Usuario generado correctamente");
            }
            return BadRequest("Ya existe este usuario");
        }

        [HttpPost("users/loginUser")]
        public IActionResult UserLogin([FromBody] UserLoginDto userLoginDto)
        {
            if (_userService.UserLogin(userLoginDto.Email, userLoginDto.Password))
            {
                return Ok($"Sesión iniciada");
            }
            return BadRequest("Email o contraseña incorrectos");
        }
    }
}
