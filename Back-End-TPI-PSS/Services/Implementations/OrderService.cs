using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly PPSContext _context;
        public OrderService(PPSContext context)
        {
            _context = context;
        }
        public Order AddOrder(OrderDto orderDto)
        {
            var orderToAdd = new Order
            {
                Id = orderDto.Id,
                UserId = orderDto.UserId,
                Status = orderDto.Status,
                Address = "Calle 202",
            };

            _context.Orders.Add(orderToAdd);
            _context.SaveChanges();
            return orderToAdd;
        }

        public bool AddProductToOrderLine(OrderLineDto orderLineDto)
        {
            var userExists = _context.Users.Any(u => u.Id == orderLineDto.UserId);
            var productExists = _context.Products.Any(p => p.Id == orderLineDto.ProductId);

            if (!userExists || !productExists)
            {
                return false;
            }

            var product = _context.Products
                .Include(p => p.Colours)
                .Include(p => p.Sizes)
                .FirstOrDefault(p => p.Id == orderLineDto.ProductId);

            if (product != null)
            {
                var selectedColour = product.Colours.FirstOrDefault(c => c.Id == orderLineDto.ColourId);
                var selectedSize = product.Sizes.FirstOrDefault(s => s.Id == orderLineDto.SizeId);

                if (selectedColour == null || selectedSize == null)
                {
                    return false;
                }

                var order = _context.Orders.FirstOrDefault(o => o.Id == orderLineDto.OrderId && o.UserId == orderLineDto.UserId);

                if (order == null)
                {
                    return false;
                }

                var orderLine = new OrderLine
                {
                    Product = product,
                    Id = product.Id,
                    ColourId = orderLineDto.ColourId,
                    SizeId = orderLineDto.SizeId,
                    Quantity = orderLineDto.Quantity,
                    OrderId = orderLineDto.OrderId,
                };

                decimal totalPriceCalculated = orderLine.Quantity * product.Price;
                order.TotalPrice += totalPriceCalculated;

                _context.OrderLines.Add(orderLine);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public List<Order> GetAllOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .Include(p => p.OrderLines)
                .ThenInclude(x => x.Colour)
                .Include(p => p.OrderLines)
                .ThenInclude(s => s.Size)
                .Include(s => s.OrderLines)
                .ThenInclude(o => o.Product)
                .ToList();
        }
    }
}
