using AutoMapper;
using BusinessObjects.Entities;
using HostelandAuthorization.Services.RoomService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            //var response = _mapper.Map<List<GetCategoryDto>>(rooms.Data);
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
