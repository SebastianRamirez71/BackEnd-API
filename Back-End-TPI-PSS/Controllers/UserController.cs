﻿using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Data.Models.UserDTOs;
using Back_End_TPI_PSS.Services.Implementations;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End_TPI_PSS.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUser(string email)
        {
            try
            {
                return Ok(_userService.GetUserByEmail(email));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] UserDto userDto)
        {
            if (_userService.CreateUser(userDto))
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest("Ya existe este usuario");
        }

        [HttpGet("users")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpDelete("users/{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            if (_userService.DeleteUser(id))
            {
                return Ok("El usuario fue eliminado correctamente");
            }
            return BadRequest("El usuario no fue encontrado");
        }
    }
}
