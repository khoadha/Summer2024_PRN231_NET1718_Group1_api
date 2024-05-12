using BusinessObjects.Entities;

namespace Repositories.ApplicationUserRepositories {
    public interface IApplicationUserRepository {
        Task<List<ApplicationUser>> GetUsers();
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<bool> IsUserExist(string id);
        bool Save();
    }
}
