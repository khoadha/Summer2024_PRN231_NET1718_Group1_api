using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.GlobalRateRepository;

namespace Hosteland.Services.GlobalRateService
{
    public class GlobalRateService : IGlobalRateService
    {
        private readonly IGlobalRateRepository _repo;

        public GlobalRateService(IGlobalRateRepository repo)
        {
            _repo = repo;
        }

        public async Task<ServiceResponse<GlobalRate>> AddGlobalRate(GlobalRate rate)
        {
            var serviceResponse = new ServiceResponse<GlobalRate>();
            try
            {
                rate.StartDate = DateTime.Now;
                var addedCate = await _repo.AddRate(rate);
                serviceResponse.Data = addedCate;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GlobalRate>>> GetGlobalRates()
        {
            var serviceResponse = new ServiceResponse<List<GlobalRate>>();
            try
            {
                var list = await _repo.GetRates();
                serviceResponse.Data = list;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GlobalRate>> GetNewestGlobalRate()
        {
            var serviceResponse = new ServiceResponse<GlobalRate>();
            try
            {
                var res = await _repo.GetNewestRateAsync();
                serviceResponse.Data = res;
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
