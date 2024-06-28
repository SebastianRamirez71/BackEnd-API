using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Models;
using Back_End_TPI_PSS.Services.Interfaces;
using MercadoPago.Resource.User;

namespace Back_End_TPI_PSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Asegura que todas las acciones del controlador requieran autenticación
    public class MercadoPagoController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PPSContext _context;
        private readonly IMercadoPagoPayment _mercadoPagoPayment;
        private readonly IOrderService _orderService;

        public MercadoPagoController(
            IHttpContextAccessor httpContextAccessor,
            IMercadoPagoPayment mercadoPagoPayment,
            IOrderService orderService,
            PPSContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _mercadoPagoPayment = mercadoPagoPayment;
            _orderService = orderService;
            _context = context;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment([FromBody] List<CartItem> items)
        {
            try
            {
                var userId = GetUserIdFromToken();

                // Crear la preferencia en MercadoPago
                var preference = await _mercadoPagoPayment.CreatePreferenceRequest(items, userId);

                // Retornar la respuesta con el ID de la preferencia creada
                return Ok(new { preferenceId = preference.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating payment preference: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return BadRequest($"Error creating payment preference: {ex.Message}");
            }
        }


        [HttpPut("paymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatus([FromQuery] string preferenceId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.PreferenceId == preferenceId);

            if (status == "approved")
            {
                await _orderService.UpdateOrderStatus(order);
                order.UpdatedAt = DateTime.Now;
                return Ok();
            }
            return NotFound();
        }
        private int GetUserIdFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                //Console.WriteLine($"Token: {token}"); // Para depuración

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken != null)
                {
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                   // Console.WriteLine($"UserId Claim: {userIdClaim}"); // Para depuración

                    if (int.TryParse(userIdClaim, out var userId))
                    {
                     //   Console.WriteLine($"Parsed UserId: {userId}"); // Para depuración
                        return userId;
                    }
                }
            }

            throw new Exception("Error retrieving userId from token.");
        }
    }
}
