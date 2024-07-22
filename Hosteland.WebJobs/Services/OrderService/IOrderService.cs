using BusinessObjects.ConfigurationModels;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
namespace Hosteland.WebJobs.Services.OrderService {
    public interface IOrderService {
        Task<ServiceResponse<List<Order>>> GetOrders();
        Task<ServiceResponse<Order>> GetOrderById(int id);
        Task<ServiceResponse<List<Order>>> GetOrdersByRoomId(int roomId);
        Task<ServiceResponse<Order>> AddOrder(Order order);
        Task<ServiceResponse<Order>> CreateOrder(Order order, List<Contract> contract, List<Fee> fee);
        Task<ServiceResponse<List<ContractType>>> GetContractTypes();
        Task<ServiceResponse<ContractType>> AddContractType(ContractType type);
        Task TriggerMonthlyBill(CancellationToken cancellationToken);
        Task<ServiceResponse<List<Fee>>> GetDeferredElectricityFee();
        Task UpdateAmountFee(UpdateAmountFeeRequestDTO dto);
        Task<ServiceResponse<List<FeeCategory>>> GetFeeCates();
        Task<ServiceResponse<FeeCategory>> AddFeeCate(FeeCategory type);
        Task<ServiceResponse<Fee>> GetFeeById(int id);
        Task<ServiceResponse<List<Fee>>> GetFeesByOrderId(int orderId);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
