using BusinessObjects.Entities;

namespace Repositories.ServiceRepository
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetServices();
        Task<Service> GetServiceById(int id);
        Task<Service> AddService(Service Service, string imgPath);
        Task<ServicePrice> CreateServicePrice(ServicePrice servicePrice);
        Task<List<ServicePrice>> GetServicePricesByServiceId(int id);
        Task<bool> SaveAsync();
    }
}
