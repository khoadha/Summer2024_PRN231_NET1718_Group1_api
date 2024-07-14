using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace Hosteland.Services.ApplicationUserService {
    public interface IApplicationUserService {
        Task<ServiceResponse<List<ApplicationUser>>> GetUsers();
        Task<ServiceResponse<ApplicationUser>> GetUserByEmail(string email);
        Task<ServiceResponse<ApplicationUser>> GetUserById(string id);
        Task<ServiceResponse<bool>> IsUserExist(string id);
        bool Save();

        Task<ServiceResponse<int>> GetUsersCount();
    }
}
