using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using MailKit.Search;
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
                        UserId = 1
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
               // await AddOrderLine(existingOrder);
                return true;
            }
            return false;
        }

        public async Task<bool> AddOrderLine(Order order)
        {
            //Product product = _context.Products.FirstOrDefault(x => x.Id == order.ProductId);
            //var stock = _context.Stocks.FirstOrDefault(x => x.ProductId == product.Id);
            //var quantity = _context.StockSizes.FirstOrDefault(x => x.StockId == stock.Id);
            var orderLine = new OrderLine
            {
                OrderId = order.Id,
                //ProductId = order.ProductId,
                //UnitPrice = product.Price,
                //Description = product.Description,
                //Quantity = quantity.Quantity
            };

            _context.OrderLines.Add(orderLine);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
