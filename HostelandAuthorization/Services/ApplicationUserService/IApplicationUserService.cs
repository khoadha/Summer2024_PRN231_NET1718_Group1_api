using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace HostelandAuthorization.Services.ApplicationUserService {
    public interface IApplicationUserService {
        Task<ServiceResponse<List<ApplicationUser>>> GetUsers();
        Task<ServiceResponse<ApplicationUser>> GetUserByEmail(string email);
        Task<ServiceResponse<bool>> IsUserExist(string id);
        bool Save();
    }
}
