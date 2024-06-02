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
        Task<Room> AddRoom(Room room, List<RoomImage> image);
        Task AddFurnitureToRoom(List<RoomFurniture> roomFurnitures);
        Task<Room> EditRoom(Room room);
        Task<bool> DeleteRoom(int roomId);
        Task<Room> FindRoomById(int id);
        Task<List<Room>> SearchRoom(string query);
    }
}
