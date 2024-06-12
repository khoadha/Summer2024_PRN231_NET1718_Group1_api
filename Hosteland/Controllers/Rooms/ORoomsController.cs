using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BusinessObjects.DTOs;
using Hosteland.Services.RoomCategoryService;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Hosteland.Services.RoomService;

namespace Hosteland.Controllers.ORoomCategories {

    [Route("odata/")]
    public class ORoomsController : ODataController {

        private readonly IMapper _mapper;
        private readonly IRoomCategoryService _roomCategoryService;
        private readonly IRoomService _roomService;

        public ORoomsController(IMapper mapper, IRoomCategoryService roomCategoryService, IRoomService roomService) {
            _mapper = mapper;
            _roomCategoryService = roomCategoryService;
            _roomService = roomService;
        }

        [HttpGet("ORoomDisplays")]
        [EnableQuery]
        public async Task<IActionResult> GetRoomsDisplay() {
            var rooms = await _roomService.GetRooms();
            var response = _mapper.Map<List<GetRoomDTO>>(rooms.Data);
            return Ok(response.AsQueryable());
        }

        [HttpGet("ORooms")]
        [EnableQuery]
        public async Task<IActionResult> GetRooms() {
            var rooms = await _roomService.GetRooms();
            var response = _mapper.Map<List<GetRoomDetailDTO>>(rooms.Data);
            return Ok(response.AsQueryable());
        }

        [HttpGet("ORoomDetails({id})")]
        [EnableQuery]
        public async Task<IActionResult> GetRoomById([FromRoute] int id) {
            var rooms = await _roomService.GetRoomById(id);
            var response = _mapper.Map<GetRoomDetailDTO>(rooms.Data);
            return Ok(response);
        }

        //Test: https://localhost:7267/odata/ORoomCategories?$select=CategoryName (Select only name) 
        [HttpGet("ORoomCategories")]
        [EnableQuery]
        public async Task<IActionResult> GetRoomCategories() {

            var cates = await _roomCategoryService.GetRoomCategories();

            var data = cates.Data;

            var response = _mapper.Map<List<GetRoomCategoryDto>>(data);

            return Ok(response.AsQueryable());
        }

        [HttpGet("ORoomCategories({id})")]
        [EnableQuery]
        public async Task<IActionResult> GetRoomCategoryById([FromRoute] int id) {

            var cates = await _roomCategoryService.GetRoomCategories();

            var data = cates.Data.SingleOrDefault(a => a.Id == id);

            var response = _mapper.Map<GetRoomCategoryDto>(data);

            return Ok(response);
        }
    }
}
