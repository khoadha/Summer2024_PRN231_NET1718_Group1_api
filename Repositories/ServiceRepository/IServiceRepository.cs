using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ServiceRepository
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetServices();
        Task<Service> GetServiceById(int id);
        Task<Service> AddService(Service Service);
        Task<ServicePrice> CreateServicePrice(ServicePrice servicePrice);
        Task<List<ServicePrice>> GetServicePricesByServiceId(int id);
        Task<bool> SaveAsync();
    }
}
