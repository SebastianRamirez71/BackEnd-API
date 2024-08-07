﻿using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public async Task<List<Order>> GetApprovedOrdersForUser(int userId)
        {

            return await _context.Orders
                .Include(o => o.OrderLines).ThenInclude(x => x.Product)
                .Include(o => o.OrderLines).ThenInclude(x => x.Size)
                .Include(x => x.OrderLines).ThenInclude(x => x.Color)
                .Where(o => o.UserId == userId && o.Status == "Approved")
                .ToListAsync();

        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _context.Orders
                 .Include(order => order.OrderLines)
                 .ToListAsync();
        }

        public async Task<List<Order>> GetAllApprovedOrders()
        {
            return await _context.Orders
                .Where(o => o.Status == "Approved")
                .ToListAsync();
        }

        public async Task<bool> AddOrder(Order order)
        {
            // Verificar existencia del usuario
            var userExists = await _context.Users.AnyAsync(u => u.Id == order.UserId);
            if (!userExists)
            {
                throw new Exception($"User with ID {order.UserId} not found.");
            }

            // Verificar existencia de los productos, colores y tamaños en las OrderLines
            foreach (var orderLine in order.OrderLines)
            {
                var productExists = await _context.Products.AnyAsync(p => p.Id == orderLine.ProductId);
                if (!productExists)
                {
                    throw new Exception($"Product with ID {orderLine.ProductId} not found.");
                }

                var colorExists = await _context.Colours.AnyAsync(c => c.Id == orderLine.ColorId);
                if (!colorExists)
                {
                    throw new Exception($"Color with ID {orderLine.ColorId} not found.");
                }

                var sizeExists = await _context.Sizes.AnyAsync(s => s.Id == orderLine.SizeId);
                if (!sizeExists)
                {
                    throw new Exception($"Size with ID {orderLine.SizeId} not found.");
                }
            }

            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateOrderStatus(Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                existingOrder.Status = "Approved";
                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
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
