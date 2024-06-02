
using AutoMapper;
using BusinessObjects.ConfigurationModels;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Repositories.RoomRepository;

namespace HostelandAuthorization.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepo, IMapper mapper, IBlobService blobService)
        {
            _roomRepository = roomRepo;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<ServiceResponse<List<Room>>> GetRooms() {
            var serviceResponse = new ServiceResponse<List<Room>>();
            var listRoom = await _roomRepository.GetRoom();
            serviceResponse.Data = listRoom;
            return serviceResponse;
        }
        public async Task<ServiceResponse<Room>> AddRoom(AddRoomDTO roomDto)
        {
            var serviceResponse = new ServiceResponse<Room>();
            try
            {
                // Convert images to RoomImage objects
                var roomImages = new List<RoomImage>();
                foreach (var file in roomDto.Files)
                {
                    string callbackUrl = await _blobService.UploadFileAsync(file);
                    RoomImage roomImage = new RoomImage
                    {
                        Url = callbackUrl
                    };
                    roomImages.Add(roomImage);
                }

                // Convert AddRoomDTO to Room entity
                var room = _mapper.Map<Room>(roomDto);

                // Call repository method
                serviceResponse.Data = await _roomRepository.AddRoom(room, roomImages);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Room>> AddFurnitureToRoom(AddFurnitureToRoomDTO addFurnitureToRoomDto)
        {
            var serviceResponse = new ServiceResponse<Room>();
            try
            {
                var room = await _roomRepository.FindRoomById(addFurnitureToRoomDto.RoomId);
                if (room == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Room not found";
                    return serviceResponse;
                }

                var newFurnitures = addFurnitureToRoomDto.Furnitures.Select(f => new RoomFurniture
                {
                    FurnitureId = f.FurnitureId,
                    RoomId = addFurnitureToRoomDto.RoomId,
                    Quantity = f.Quantity
                }).ToList();

                await _roomRepository.AddFurnitureToRoom(newFurnitures);

                serviceResponse.Data = room;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

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
