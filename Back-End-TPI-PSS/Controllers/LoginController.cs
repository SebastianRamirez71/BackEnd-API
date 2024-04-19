using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Services.Implementations;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _service;
        public LoginController(IConfiguration config, IUserService service)
        {
            _config = config;
            _service = service;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] UserLoginDto loginDto)
        {
            if (_service.UserLogin(loginDto))
            {
                User user = _service.GetUserByEmail(loginDto.Email);

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signature = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                //Los claims son datos en clave->valor que nos permite guardar data del usuario.
                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("email", user.Email));
                claimsForToken.Add(new Claim("name", user.Name));
                claimsForToken.Add(new Claim("role", user.UserType));


                var jwtSecurityToken = new JwtSecurityToken( //agregar using System.IdentityModel.Tokens.Jwt; Acá es donde se crea el token con toda la data que le pasamos antes.
                    _config["Authentication:Issuer"],
                    _config["Authentication:Audience"],
                    claimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1),
                    signature);

                string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return Ok(tokenToReturn);
            }
            return BadRequest();           
        }
    }
}
