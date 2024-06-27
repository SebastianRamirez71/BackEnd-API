using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly PPSContext _context;

        public OrderService(PPSContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(Order order)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderToAdd = new Order
                    {
                        CreatedAt = DateTime.Now,
                        PreferenceId = order.PreferenceId,
                        Status = OrderStatus.Pending,
                        UpdatedAt = DateTime.Now, 
                        UserId =1
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
    }
}
