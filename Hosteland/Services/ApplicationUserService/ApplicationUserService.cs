using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.ApplicationUserRepositories;

namespace Hosteland.Services.ApplicationUserService {
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

        public async Task<ServiceResponse<int>> GetUsersCount()
        {
            var serviceResponse = new ServiceResponse<int>();
            var count = await _userRepo.GetUsersCount();
            serviceResponse.Data = count;
            return serviceResponse;
        }

        public async Task<ServiceResponse<ApplicationUser>> GetUserByEmail(string email) {
            var serviceResponse = new ServiceResponse<ApplicationUser>();
            var user = await _userRepo.GetUserByEmail(email);
            serviceResponse.Data = user;
            return serviceResponse;
        }

        public async Task<ServiceResponse<ApplicationUser>> GetUserById(string id)
        {
            var serviceResponse = new ServiceResponse<ApplicationUser>();
            try
            {
                var user = await _userRepo.GetUserById(id);
                serviceResponse.Data = user;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
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
