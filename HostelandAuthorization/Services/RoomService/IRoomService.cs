using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace HostelandAuthorization.Services.RoomService
{
    public interface IRoomService
    {
        Task<ServiceResponse<List<Room>>> GetRooms();
        Task<ServiceResponse<Room>> AddRoom(Room room);
    }
}
