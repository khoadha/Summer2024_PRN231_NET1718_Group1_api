using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace HostelandAuthorization.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<Order>>> GetOrders();
        Task<ServiceResponse<Order>> AddOrder(Order order);
        Task<ServiceResponse<Order>> CreateOrder(Order order, Contract contract);
    }
}
