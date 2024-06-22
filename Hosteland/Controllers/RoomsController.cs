using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.RoomService;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers.Rooms
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;

        public RoomsController(IMapper mapper, IRoomService roomService)
        {
            _mapper = mapper;
            _roomService = roomService;
        }

        [HttpGet]
        [Route("search-room/{query}")]
        public async Task<ActionResult<List<Room>>> GetRooms([FromRoute] string query)
        {
            var rooms = await _roomService.SearchRooms(query);
            var response = _mapper.Map<List<GetRoomDetailDTO>>(rooms.Data);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById([FromRoute] int id) {
            var room = await _roomService.GetRoomById(id);
            var response = _mapper.Map<GetRoomDetailDTO>(room.Data);
            return Ok(response);
        } 

        [HttpPost]
        [Route("add-room")]
        public async Task<IActionResult> AddRoom([FromForm] AddRoomDTO roomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var serviceResponse = await _roomService.AddRoom(roomDto);
            var response = _mapper.Map<GetRoomDetailDTO>(serviceResponse.Data);
            return Ok(response);

        }

        [HttpPost]
        [Route("add-furniture-to-room")]
        public async Task<IActionResult> AddFurnitureToRoom([FromBody] AddFurnitureToRoomDTO addFurnitureToRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var serviceResponse = await _roomService.AddFurnitureToRoom(addFurnitureToRoomDto);
            if (!serviceResponse.Success)
            {
                return BadRequest(new { Result = false, serviceResponse.Message });
            }

            var response = _mapper.Map<GetRoomDetailDTO>(serviceResponse.Data);
            return Ok(response);
        }


        [HttpPut]
        [Route("update-room")]
        public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomDTO updateRoomDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var serviceResponse = await _roomService.UpdateRoom(updateRoomDto);
            if (!serviceResponse.Success)
            {
                return BadRequest(new { Result = false, serviceResponse.Message });
            }

            var response = _mapper.Map<GetRoomDetailDTO>(serviceResponse.Data);
            return Ok(response);
        }
    }
}
