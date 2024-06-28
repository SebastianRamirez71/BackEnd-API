using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IOrderService
    {
        //public bool AddProductToOrderLine(OrderLineDto orderLineDto);

        public Task<bool> AddOrder(Order order);
        public Task<bool> UpdateOrderStatus(Order order);

        //public bool AddOrder(OrderDto orderDto);
        //public List<Order> GetAllOrders(int userId);
    }
}
