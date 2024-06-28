using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.OrderDTOs;
using Back_End_TPI_PSS.Models;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<bool> AddOrder(Order order);
        public Task<bool> UpdateOrderStatus(Order order);
        public Task<List<Order>> GetApprovedOrdersForUser(int userId);
        public Task<List<Order>> GetAllOrders();
        public Task<List<Order>> GetAllApprovedOrders();

    }
}
