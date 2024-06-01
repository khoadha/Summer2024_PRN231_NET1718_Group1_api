using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoomCategoryRepository
{
    public class RoomCategoryRepository : IRoomCategoryRepository
    {
        private readonly AppDbContext _context;

        public RoomCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoomCategory> AddRoomCategory(RoomCategory RoomCategory)
        {
            try
            {
                await _context.RoomCategories.AddAsync(RoomCategory);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return RoomCategory;
        }

        public async Task<List<RoomCategory>> GetRoomCategories()
        {
            List<RoomCategory> result = new List<RoomCategory>();
            try
            {
                result = await _context.RoomCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;

        }
    }
}
