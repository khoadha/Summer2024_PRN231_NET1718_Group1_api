using AutoMapper;
using BusinessObjects.Entities;
using HostelandAuthorization.Services.RoomService;
using Microsoft.AspNetCore.Mvc;

namespace HostelandAuthorization.Controllers
{
    [ApiController]
    [Route("api/v1/")]

    public class RoomController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;

        public RoomController (IMapper mapper, IRoomService roomService)
        {
            _mapper = mapper;
            _roomService = roomService;
        }

        [HttpGet]
        [Route("room/get-room")]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _roomService.GetRooms();
            return Ok(rooms);
        }

        [HttpPost]
        [Route("room/add")]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var createdRoom = await _roomService.AddRoom(room);
            return Ok(createdRoom);
        }
    }
}
