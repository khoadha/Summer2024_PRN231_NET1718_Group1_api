using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoomRepository
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetRoom();
        Task<Room> AddRoom(Room room);
        Task<Room> EditRoom(Room room);
        Task<bool> DeleteRoom(int roomId);
        Task<Room> FindRoomById(int id);
    }
}
