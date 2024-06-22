
using AutoMapper;
using BusinessObjects.ConfigurationModels;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Repositories.RoomCategoryRepository;
using Repositories.RoomRepository;

namespace Hosteland.Services.RoomService
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IRoomCategoryRepository _roomCategoryRepository;

        public RoomService(IRoomRepository roomRepo, IMapper mapper, IBlobService blobService, IRoomCategoryRepository roomCategoryRepository)
        {
            _roomRepository = roomRepo;
            _mapper = mapper;
            _blobService = blobService;
            _roomCategoryRepository = roomCategoryRepository;
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

                // Fetch the Category entity based on the CategoryId
                var category = await _roomCategoryRepository.GetRoomCategoryById(roomDto.CategoryId);
                if (category == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Category not found";
                    return serviceResponse;
                }

                // Convert AddRoomDTO to Room entity
                var room = _mapper.Map<Room>(roomDto);
                room.Category = category;

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

                foreach (var furnitureDto in addFurnitureToRoomDto.Furnitures)
                {
                    // Check if the furniture already exists in the room
                    var existingFurniture = room.RoomFurniture.FirstOrDefault(f => f.FurnitureId == furnitureDto.FurnitureId);
                    if (existingFurniture != null)
                    {
                        if (furnitureDto.Quantity > 0)
                        {
                            existingFurniture.Quantity = furnitureDto.Quantity;
                        }
                        else
                        {
                            room.RoomFurniture.Remove(existingFurniture);
                        }
                    }
                    else
                    if (furnitureDto.Quantity > 0)
                    {
                        var newFurniture = new RoomFurniture
                        {
                            FurnitureId = furnitureDto.FurnitureId,
                            RoomId = addFurnitureToRoomDto.RoomId,
                            Quantity = furnitureDto.Quantity
                        };
                        room.RoomFurniture.Add(newFurniture);
                    }
                }

                await _roomRepository.EditRoom(room);

                serviceResponse.Data = room;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Room>> UpdateRoom(UpdateRoomDTO updateRoomDto)
        {
            var serviceResponse = new ServiceResponse<Room>();
            try
            {
                var roomToUpdate = await _roomRepository.FindRoomById(updateRoomDto.RoomId);
                if (roomToUpdate == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Room not found";
                    return serviceResponse;
                }

                // Update room properties if provided in the DTO
                if (!string.IsNullOrEmpty(updateRoomDto.Name))
                    roomToUpdate.Name = updateRoomDto.Name;
                if (updateRoomDto.RoomSize != default)
                    roomToUpdate.RoomSize = updateRoomDto.RoomSize;
                if (updateRoomDto.RoomArea != default)
                    roomToUpdate.RoomArea = updateRoomDto.RoomArea;
                if (!string.IsNullOrEmpty(updateRoomDto.RoomDescription))
                    roomToUpdate.RoomDescription = updateRoomDto.RoomDescription;
                if (updateRoomDto.CostPerDay != default)
                    roomToUpdate.CostPerDay = updateRoomDto.CostPerDay;
                if (!string.IsNullOrEmpty(updateRoomDto.Location))
                    roomToUpdate.Location = updateRoomDto.Location;
                if (updateRoomDto.CategoryId != default)
                    roomToUpdate.CategoryId = updateRoomDto.CategoryId;

                // Fetch the Category entity based on the CategoryId
                var category = await _roomCategoryRepository.GetRoomCategoryById(roomToUpdate.CategoryId);
                if (category == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Category not found";
                    return serviceResponse;
                }

                roomToUpdate.Category = category;
                await _roomRepository.EditRoom(roomToUpdate);

                serviceResponse.Data = roomToUpdate;
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
