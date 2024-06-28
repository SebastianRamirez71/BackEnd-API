using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly PPSContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(PPSContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddOrder(Order order)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int userId = GetUserIdFromToken(); // Obtener el userId desde el token JWT

                    var orderToAdd = new Order
                    {
                        CreatedAt = DateTime.Now,
                        PreferenceId = order.PreferenceId,
                        ProductQuantity = order.ProductQuantity,
                        Status = OrderStatus.Pending,
                        UpdatedAt = DateTime.Now,
                        ProductId = order.ProductId,
                        UserId = userId // Asignar el userId obtenido
                    };

                    _context.Orders.Add(orderToAdd);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding order: {ex.Message}");
                    await transaction.RollbackAsync();
                    throw new Exception("Error adding order", ex);
                }
            }
        }

        public async Task<bool> UpdateOrderStatus(Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                existingOrder.Status = OrderStatus.Approved;
                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AddOrderLine(Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.Id == order.Id);

            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == order.ProductId);
            if (product == null)
            {
                throw new Exception($"Product with ID {order.ProductId} not found.");
            }

            var orderLine = new OrderLine
            {
                OrderId = existingOrder.Id,
                ProductId = product.Id,
                PreferenceId = existingOrder.PreferenceId,
                Description = product.Description,
                Quantity = existingOrder.ProductQuantity,
                UnitPrice = product.Price
            };

            _context.OrderLines.Add(orderLine);
            await _context.SaveChangesAsync();

            return true;
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
