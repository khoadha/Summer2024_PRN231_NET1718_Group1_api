﻿using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace HostelandAuthorization.Services.ServiceService
{
    public interface IServiceService
    {
        Task<ServiceResponse<List<Service>>> GetServices();
        Task<ServiceResponse<Service>> AddService(Service service);
        Task<ServiceResponse<ServicePrice>> CreateServicePrice(ServicePrice servicePrice);
        Task<ServiceResponse<List<ServicePrice>>> GetServicePricesByServiceId(int id);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
