using BusinessObjects.ConfigurationModels;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;

namespace HostelandOData.Services.RoomService
{
    public interface IRoomService
    {
        Task<ServiceResponse<List<Room>>> GetRooms();
        Task<ServiceResponse<Room>> GetRoomById(int id);
        Task<ServiceResponse<List<Room>>> SearchRooms(string query);
    }
}
