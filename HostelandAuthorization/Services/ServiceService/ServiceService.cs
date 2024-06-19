using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.ServiceRepository;

namespace Hosteland.Services.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repo;

        public ServiceService(IServiceRepository repo)
        {
            _repo = repo;
        }

        public async Task<ServiceResponse<Service>> AddService(Service Service)
        {
            var serviceResponse = new ServiceResponse<Service>();
            try
            {
                var addedCate = await _repo.AddService(Service);
                serviceResponse.Data = addedCate;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<ServicePrice>> CreateServicePrice(ServicePrice servicePrice)
        {
            var serviceResponse = new ServiceResponse<ServicePrice>();
            try
            {
                var serviceExist = await _repo.GetServiceById(servicePrice.ServiceId);
                if(serviceExist == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Service not found";
                    return serviceResponse;
                }
                servicePrice.StartDate = DateTime.Now;

                var added = await _repo.CreateServicePrice(servicePrice);
                serviceResponse.Data = added;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Service>>> GetServices()
        {
            var serviceResponse = new ServiceResponse<List<Service>>();
            try
            {
                var list = await _repo.GetServices();
                serviceResponse.Data = list;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<List<ServicePrice>>> GetServicePricesByServiceId(int id)
        {
            var serviceResponse = new ServiceResponse<List<ServicePrice>>();
            try
            {
                var serviceExist = await _repo.GetServiceById(id);
                if (serviceExist == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Service not found";
                    return serviceResponse;
                }
                var list = await _repo.GetServicePricesByServiceId(id);
                serviceResponse.Data = list;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<ServicePrice>> GetServiceNewestPricesByServiceId(int id)
        {
            var serviceResponse = new ServiceResponse<ServicePrice>();
            try
            {
                var serviceExist = await _repo.GetServiceById(id);
                if (serviceExist == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Service not found";
                    return serviceResponse;
                }
                var per = _repo.GetServicePricesByServiceId(id).Result.OrderByDescending(a => a.StartDate).FirstOrDefault();
                serviceResponse.Data = per;
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
                var result = await _repo.SaveAsync();
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
