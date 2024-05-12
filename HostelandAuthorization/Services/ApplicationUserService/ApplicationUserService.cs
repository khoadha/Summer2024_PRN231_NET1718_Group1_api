using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.ApplicationUserRepositories;

namespace HostelandAuthorization.Services.ApplicationUserService {
    public class ApplicationUserService : IApplicationUserService {

        private readonly IApplicationUserRepository _userRepo;

        public ApplicationUserService(IApplicationUserRepository userRepo) {
            _userRepo = userRepo;
        }

        public async Task<ServiceResponse<List<ApplicationUser>>> GetUsers() {
            var serviceResponse = new ServiceResponse<List<ApplicationUser>>();
            var listUsers = await _userRepo.GetUsers();
            serviceResponse.Data = listUsers;
            return serviceResponse;
        }

        public async Task<ServiceResponse<ApplicationUser>> GetUserByEmail(string email) {
            var serviceResponse = new ServiceResponse<ApplicationUser>();
            var user = await _userRepo.GetUserByEmail(email);
            serviceResponse.Data = user;
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> IsUserExist(string id) {
            var serviceResponse = new ServiceResponse<bool>();
            var result = await _userRepo.IsUserExist(id);
            serviceResponse.Data = result;
            return serviceResponse;
        }

        public bool Save() {
            return _userRepo.Save();
        }
    }
}
