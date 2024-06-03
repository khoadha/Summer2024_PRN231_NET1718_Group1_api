using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories.ApplicationUserRepositories {
    public class ApplicationUserRepository : IApplicationUserRepository {

        private readonly AppDbContext _context;

        public ApplicationUserRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetUsers() {
            var listUsers = await _context.Users.ToListAsync();
            return listUsers;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email) {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.NormalizedEmail == email.ToUpper());
            return user;
        }
      
        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
            return user;
        }

        public async Task<bool> IsUserExist(string id) {
            var result = await _context.Users.AnyAsync(a => a.Id == id);
            return result;
        }

        public bool Save() {
            int save = _context.SaveChanges();
            return save > 0;
        }
    }
}
