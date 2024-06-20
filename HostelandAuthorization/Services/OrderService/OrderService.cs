
using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.OrderRepository;

namespace Hosteland.Services.OrderService
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
        public async Task<ServiceResponse<Order>> GetOrderById(int id)
        {
            var serviceResponse = new ServiceResponse<Order>();
            var listOrder = await _orderRepository.GetOrderById(id);
            serviceResponse.Data = listOrder;
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<Order>>> GetOrdersByRoomId(int roomId)
        {
            var serviceResponse = new ServiceResponse<List<Order>>();
            var listOrder = await _orderRepository.GetOrdersByRoomId(roomId);
            serviceResponse.Data = listOrder;
            return serviceResponse;
        }
        public async Task<ServiceResponse<Order>> AddOrder(Order order)
        {
            var serviceResponse = new ServiceResponse<Order>();
            serviceResponse.Data = await _orderRepository.AddOrder(order);
            return serviceResponse;
        }
        public async Task<ServiceResponse<Order>> CreateOrder(Order order, List<Contract> contract)
        {
            var serviceResponse = new ServiceResponse<Order>();
            serviceResponse.Data = await _orderRepository.CreateOrder(order,contract);
            return serviceResponse;
        }

        public async Task<ServiceResponse<ContractType>> AddContractType(ContractType RoomCategory)
        {
            var serviceResponse = new ServiceResponse<ContractType>();
            try
            {
                var addedCate = await _orderRepository.AddContractType(RoomCategory);
                serviceResponse.Data = addedCate;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }



        public async Task<ServiceResponse<List<ContractType>>> GetContractTypes()
        {
            var serviceResponse = new ServiceResponse<List<ContractType>>();
            try
            {
                var list = await _orderRepository.GetContractTypes();
                serviceResponse.Data = list;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> SaveAsync()
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var result = await _orderRepository.SaveAsync();
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
