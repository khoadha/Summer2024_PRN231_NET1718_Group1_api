using BusinessObjects.Entities;

namespace Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
        Task<List<Order>> GetOrdersByRoomId(int roomId);
        Task<Order> AddOrder(Order order);
        Task<Order> CreateOrder(Order order, Contract contract);
        Task<Order> EditOrder(Order order);
        Task<bool> DeleteOrder(int orderId);
        Task<Order> FindOrderById(int id);
        Task<Order> GetOrderById(int id);

        Task<List<ContractType>> GetContractTypes();
        Task<ContractType> AddContractType(ContractType type);
        Task<bool> SaveAsync();


    }
}
