
using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using Repositories.RoomRepository;

namespace HostelandAuthorization.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepo)
        {
            _roomRepository = roomRepo;
        }

        public async Task<ServiceResponse<List<Room>>> GetRooms() {
            var serviceResponse = new ServiceResponse<List<Room>>();
            var listRoom = await _roomRepository.GetRoom();
            serviceResponse.Data = listRoom;
            return serviceResponse;
        }
        public async Task<ServiceResponse<Room>> AddRoom(Room room)
        {
            var serviceResponse = new ServiceResponse<Room>();          
            serviceResponse.Data = await _roomRepository.AddRoom(room);
            return serviceResponse;
        }
    }
}
