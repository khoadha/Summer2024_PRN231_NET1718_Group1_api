using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace Hosteland.Services.GlobalRateService
{
    public interface IGlobalRateService
    {
        Task<ServiceResponse<List<GlobalRate>>> GetGlobalRates();
        Task<ServiceResponse<GlobalRate>> AddGlobalRate(GlobalRate rate);
        Task<ServiceResponse<GlobalRate>> GetNewestGlobalRate();
        Task<ServiceResponse<bool>> SaveAsync();
    }
}
