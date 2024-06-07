using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace Hosteland.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<Order>>> GetOrders();
        Task<ServiceResponse<List<Order>>> GetOrdersByRoomId(int roomId);
        Task<ServiceResponse<Order>> AddOrder(Order order);
        Task<ServiceResponse<Order>> CreateOrder(Order order, Contract contract);
    }
}
