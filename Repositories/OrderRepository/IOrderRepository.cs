using BusinessObjects.Entities;

namespace Repositories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders();
        Task<List<Order>> GetOrdersByRoomId(int roomId);
        Task<Order> AddOrder(Order order);
        Task<Order> CreateOrder(Order order, List<Contract> contract, List<Fee> fee);
        Task<Order> EditOrder(Order order);
        Task<bool> DeleteOrder(int orderId);
        Task<Order> FindOrderById(int id);
        Task<Order> GetOrderById(int id);
        Task<List<ContractType>> GetContractTypes();
        Task<ContractType> AddContractType(ContractType type);
        Task<List<FeeCategory>> GetFeeCates();
        Task<FeeCategory> AddFeeCate(FeeCategory type);
        Task<List<Fee>> GetFees();
        Task<List<Fee>> GetFeesByOrderId(int orderId);
        Task<bool> SaveAsync();


    }
}
