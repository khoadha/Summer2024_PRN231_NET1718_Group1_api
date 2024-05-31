
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
        public async Task<ServiceResponse<Room>> AddRoom(AddRoomDTO roomDto, List<IFormFile> images)
        {
            var serviceResponse = new ServiceResponse<Room>();
            try
            {
                // Convert images to RoomImage objects
                var roomImages = new List<RoomImage>();
                foreach (var file in images)
                {
                    string callbackUrl = await _blobService.UploadFileAsync(file);
                    RoomImage roomImage = new RoomImage
                    {
                        Url = callbackUrl
                    };
                    roomImages.Add(roomImage);
                }

                // Convert Furnitures from DTO to RoomFurniture entities
                var roomFurnitures = roomDto.Furnitures.Select(f => new RoomFurniture
                {
                    FurnitureId = f.FurnitureId,
                    Quantity = f.Quantity
                }).ToList();

                // Convert AddRoomDTO to Room entity
                var room = new Room
                {
                    Name = roomDto.Name,
                    RoomSize = roomDto.RoomSize,
                    RoomArea = roomDto.RoomArea,
                    RoomDescription = roomDto.RoomDescription,
                    CostPerDay = roomDto.CostPerDay,
                    Location = roomDto.Location,
                    CategoryId = roomDto.CategoryId,
                };

                // Call repository method
                serviceResponse.Data = await _roomRepository.AddRoom(room, roomImages, roomFurnitures);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

    }
}
