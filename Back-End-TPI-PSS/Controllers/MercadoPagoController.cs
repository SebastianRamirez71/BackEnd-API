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
                var userId = GetUserIdFromToken();  // Obtener el userId desde el token JWT

                var preference = await _mercadoPagoPayment.CreatePreferenceRequest(items);

                var item = items.First(); // Tomar el primer elemento para simplificar

                // Crear el objeto Order con el UserId correctamente asignado
                var order = new Order
                {
                    PreferenceId = preference.Id.ToString(),
                    ProductQuantity = item.Quantity, // Cantidad total de productos en el pedido
                    OrderLines = new List<OrderLine>
            {
                new OrderLine
                {
                    Description = item.Name,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    ColorId = item.ColorId,
                    SizeId = item.SizeId
                }
            },
                    UserId = userId  // Asignar el userId recuperado desde el token
                };

                await _orderService.AddOrder(order);

                Console.WriteLine("Order created successfully");
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

        [HttpPost("postOrderLine")]
        public async Task<IActionResult> AddOrderLine([FromQuery] string preferenceId, string status)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.PreferenceId == preferenceId);

            if (status == "approved")
            {
                await _orderService.AddOrderLine(order);
                order.UpdatedAt = DateTime.Now;
                return Ok();
            }
            return NotFound();
        }

        // Método para obtener el userId desde el token JWT
        private int GetUserIdFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken != null && jwtToken.Claims.FirstOrDefault(c => c.Type == "sub") != null)
                {
                    var userId = int.Parse(jwtToken.Claims.FirstOrDefault(c => c.Type == "sub").Value);
                    return userId;
                }
            }

            throw new Exception("Error retrieving userId from token.");
        }
    }
}
