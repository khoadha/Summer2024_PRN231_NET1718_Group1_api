using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using HostelandAuthorization.Services.RoomService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostelandAuthorization.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class RoomsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;

        public RoomsController (IMapper mapper, IRoomService roomService)
        {
            _mapper = mapper;
            _roomService = roomService;
        }

        [HttpGet]
        [Route("get-room")]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _roomService.GetRooms();
            var response = _mapper.Map<List<GetRoomDTO>>(rooms.Data);
            return Ok(response);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddRoom([FromForm] AddRoomDTO roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var serviceResponse = await _roomService.AddRoom(room);
            var response = _mapper.Map<GetRoomDTO>(serviceResponse.Data);
            return Ok(response);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom([FromRoute]int id, [FromBody] Room room)
        {
            //if (id != room.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(room).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!RoomExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }
    }
}
