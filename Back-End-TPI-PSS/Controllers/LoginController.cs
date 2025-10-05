using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Data.Models.UserDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Back_End_TPI_PSS.Controllers
{
    public class TokenModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        public LoginController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLoginDto userLoginDto)
        {
            if (_userService.UserLogin(userLoginDto))
            {
                User user = _userService.GetUserByEmail(userLoginDto.Email);
                string token = GenerateJwtToken(user);

                var refreshToken = GenerateRefreshToken();
                var expiryTime = DateTime.UtcNow.AddDays(2);

                user.RefreshToken = refreshToken;
                user.Expiry = expiryTime;

                _userService.UpdateUser(user);

                return Ok(new TokenModel
                {
                    AccessToken = token,
                    RefreshToken = refreshToken
                });
            }
            return BadRequest(new { Message = "Invalid credentials" });
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] TokenModel tokenModel)
        {
            // 1. Validar el Access Token expirado para extraer el principal (Claims)
            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) 
            {
                return NotFound("User not found");
            }

            var user = _userService.GetUserById(userId);

            // 2. Validar el Refresh Token almacenado en la tabla de usuarios
            if (user == null || user.RefreshToken != refreshToken || user.Expiry <= DateTime.UtcNow)
            {
                // Si falla la validación, requiere nuevo inicio de sesión.
                return Unauthorized("Invalid client request or expired refresh token");
            }

            // 3. Generar un nuevo Access Token
            var newAccessToken = GenerateJwtToken(user);

            // 4. (Opcional, pero recomendado) Rotación: generar un nuevo Refresh Token y actualizarlo en la DB
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.Expiry = DateTime.UtcNow.AddDays(7);

            _userService.UpdateUser(user);

            return Ok(new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            if (_userService.CreateUser(userDto))
            {
                return Ok(new { Message = "User registered successfully" });
            }
            return BadRequest(new { Message = "Email already in use" });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("name", user.Name),
            new Claim("surname", user.SurName),
            new Claim("role", user.UserType),
            new Claim(type:"subscribe", value:user.Notification.ToString().ToLower(),valueType:ClaimValueTypes.Boolean)
        };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            try
            {
                // Valida el token, pero ignora la expiración (debido a ValidateLifetime = false)
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                // Se asegura de que el token original sea un JWT y usa el algoritmo esperado
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception)
            {
                // Si la validación falla por cualquier otra razón (firma incorrecta, etc.)
                return null;
            }
        }

    }
}