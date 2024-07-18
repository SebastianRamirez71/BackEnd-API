using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Endpoint para ver las órdenes del usuario autenticado en estado aprobado
        [HttpGet("user/approved")]
        public async Task<IActionResult> GetUserApprovedOrders()
        {
            try
            {
                // Obtener el userId del token
                var userId = GetUserIdFromToken();

                var orders = await _orderService.GetApprovedOrdersForUser(userId);

                if (orders == null || !orders.Any())
                {
                    return NotFound("No se encontraron órdenes aprobadas para este usuario.");
                }
                await _orderService.GetAllOrders();
                return Ok(orders);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener órdenes aprobadas: {ex.Message}");
            }
        }

        // Endpoint para ver todas las órdenes (requiere rol de administrador)
        [HttpGet("admin/all")]
        [Authorize(Policy = "AdminEmployee")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrders();

                if (orders == null || !orders.Any())
                {
                    return NotFound("No se encontraron órdenes.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener todas las órdenes: {ex.Message}");
            }
        }

        // Endpoint para ver todas las órdenes aprobadas (requiere rol de administrador)
        [HttpGet("admin/approved")]
        [Authorize(Roles = "AdminEmployee")]
        public async Task<IActionResult> GetAllApprovedOrders()
        {
            try
            {
                var orders = await _orderService.GetAllApprovedOrders();

                if (orders == null || !orders.Any())
                {
                    return NotFound("No se encontraron órdenes aprobadas.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al obtener todas las órdenes aprobadas: {ex.Message}");
            }
        }

        // Método privado para obtener el userId desde el token JWT
        private int GetUserIdFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new ApplicationException("No se pudo obtener el UserId desde el token JWT.");
        }
    }
}
