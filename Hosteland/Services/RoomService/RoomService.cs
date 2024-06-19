
using AutoMapper;
using BusinessObjects.ConfigurationModels;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Repositories.RoomCategoryRepository;
using Repositories.RoomRepository;

namespace HostelandOData.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IRoomCategoryRepository _roomCategoryRepository;

        public RoomService(IRoomRepository roomRepo, IMapper mapper, IRoomCategoryRepository roomCategoryRepository)
        {
            _roomRepository = roomRepo;
            _mapper = mapper;
            _roomCategoryRepository = roomCategoryRepository;
        }

        public async Task<ServiceResponse<List<Room>>> GetRooms() {
            var serviceResponse = new ServiceResponse<List<Room>>();
            var listRoom = await _roomRepository.GetRoom();
            serviceResponse.Data = listRoom;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Room>> GetRoomById(int id)
        {
            var serviceResponse = new ServiceResponse<Room>();
            var room = await _roomRepository.FindRoomById(id);
            serviceResponse.Data = room;
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<Room>>> SearchRooms(string query)
        {
            var serviceResponse = new ServiceResponse<List<Room>>();
            var listRoom = await _roomRepository.SearchRoom(query);
            serviceResponse.Data = listRoom;
            return serviceResponse;
        }

    }
}
