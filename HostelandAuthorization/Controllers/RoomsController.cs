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
            var response = _mapper.Map<List<GetRoomDetailDTO>>(rooms.Data);
            return Ok(response);
        }

        [HttpGet]
        [Route("get-room/{id}")]
        public async Task<ActionResult<List<Room>>> GetRoomById([FromRoute] int id)
        {
            var rooms = await _roomService.GetRoomById(id);
            var response = _mapper.Map<List<GetRoomDetailDTO>>(rooms.Data);
            return Ok(response);
        }
        [HttpGet]
        [Route("search-room/{query}")]
        public async Task<ActionResult<List<Room>>> GetRooms([FromRoute] string query)
        {
            var rooms = await _roomService.SearchRooms(query);
            var response = _mapper.Map<List<GetRoomDetailDTO>>(rooms.Data);
            return Ok(response);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddRoom([FromForm] AddRoomDTO roomDto)
        {
            //var room = _mapper.Map<Room>(roomDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var serviceResponse = await _roomService.AddRoom(roomDto);
            var response = _mapper.Map<GetRoomDetailDTO>(serviceResponse.Data);
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
