﻿using BusinessObjects.ConfigurationModels;
using BusinessObjects.DTOs;
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
        public async Task<ServiceResponse<List<Order>>> GetOrdersDisplay()
        {
            var serviceResponse = new ServiceResponse<List<Order>>();
            var listOrder = await _orderRepository.GetOrdersDisplay();
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
            try
            {
                var listOrder = await _orderRepository.GetOrdersByRoomId(roomId);
                serviceResponse.Data = listOrder;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<Order>> AddOrder(Order order)
        {
            var serviceResponse = new ServiceResponse<Order>();
            serviceResponse.Data = await _orderRepository.AddOrder(order);
            return serviceResponse;
        }
        public async Task<ServiceResponse<Order>> CreateOrder(Order order, List<Contract> contract, List<Fee> fee)
        {
            var serviceResponse = new ServiceResponse<Order>();
            serviceResponse.Data = await _orderRepository.CreateOrder(order, contract, fee);
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

        public async Task<ServiceResponse<FeeCategory>> AddFeeCate(FeeCategory cate)
        {
            var serviceResponse = new ServiceResponse<FeeCategory>();
            try
            {
                var addedCate = await _orderRepository.AddFeeCate(cate);
                serviceResponse.Data = addedCate;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<FeeCategory>>> GetFeeCates()
        {
            var serviceResponse = new ServiceResponse<List<FeeCategory>>();
            try
            {
                var list = await _orderRepository.GetFeeCates();
                serviceResponse.Data = list;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Fee>>> GetFeesByOrderId(int orderId)
        {
            var serviceResponse = new ServiceResponse<List<Fee>>();
            try
            {
                var list = await _orderRepository.GetFeesByOrderId(orderId);
                serviceResponse.Data = list;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Fee>>> GetDeferredElectricityFee() {
            var serviceResponse = new ServiceResponse<List<Fee>>();
            try {
                var list = await _orderRepository.GetDeferredElectricityFee();
                serviceResponse.Data = list;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task UpdateAmountFee(UpdateAmountFeeRequestDTO dto) {
               await _orderRepository.UpdateAmountFee(dto);
        }

        public async Task<ServiceResponse<Fee>> GetFeeById(int id)
        {
            var serviceResponse = new ServiceResponse<Fee>();
            try
            {
                var list = await _orderRepository.GetFeeById(id);
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

        public async Task TriggerMonthlyBill(CancellationToken cancellationToken) {
            await _orderRepository.TriggerMonthlyBill(cancellationToken);
        }
    }
}
