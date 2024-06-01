
using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.OrderRepository;

namespace HostelandAuthorization.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepo)
        {
            _orderRepository = orderRepo;
        }

        public async Task<ServiceResponse<List<Order>>> GetOrders()
        {
            var serviceResponse = new ServiceResponse<List<Order>>();
            var listOrder = await _orderRepository.GetOrders();
            serviceResponse.Data = listOrder;
            return serviceResponse;
        }
        public async Task<ServiceResponse<Order>> AddOrder(Order order)
        {
            var serviceResponse = new ServiceResponse<Order>();
            serviceResponse.Data = await _orderRepository.AddOrder(order);
            return serviceResponse;
        }
        public async Task<ServiceResponse<Order>> CreateOrder(Order order, Contract contract)
        {
            var serviceResponse = new ServiceResponse<Order>();
            serviceResponse.Data = await _orderRepository.CreateOrder(order,contract);
            return serviceResponse;
        }
    }
}
