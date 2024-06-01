using BusinessObjects.Entities;

namespace Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
        Task<Order> AddOrder(Order order);
        Task<Order> CreateOrder(Order order, Contract contract);
        Task<Order> EditOrder(Order order);
        Task<bool> DeleteOrder(int orderId);
        Task<Order> FindOrderById(int id);
    }
}
