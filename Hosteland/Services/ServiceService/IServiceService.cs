using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace Hosteland.Services.ServiceService
{
    public interface IServiceService
    {
        Task<ServiceResponse<List<Service>>> GetServices();
        Task<ServiceResponse<Service>> GetServiceById(int id);
        Task<ServiceResponse<Service>> AddService(Service service, IFormFile imgFile);
        Task<ServiceResponse<ServicePrice>> CreateServicePrice(ServicePrice servicePrice);
        Task<ServiceResponse<List<ServicePrice>>> GetServicePricesByServiceId(int id);
        Task<ServiceResponse<ServicePrice>> GetServiceNewestPricesByServiceId(int id);
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
