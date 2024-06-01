using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoomCategoryRepository
{
    public interface IRoomCategoryRepository
    {
        Task<List<RoomCategory>> GetRoomCategories();
        Task<RoomCategory> AddRoomCategory(RoomCategory RoomCategory);
        Task<bool> SaveAsync();

    }
}
