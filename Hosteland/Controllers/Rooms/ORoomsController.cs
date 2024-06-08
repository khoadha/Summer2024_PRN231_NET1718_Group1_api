using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BusinessObjects.DTOs;
using Hosteland.Services.RoomCategoryService;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Hosteland.Controllers.ORoomCategories {

    [Route("odata/")]
    public class ORoomCategoriesController : ODataController {

        private readonly IMapper _mapper;
        private readonly IRoomCategoryService _roomCategoryService;

        public ORoomCategoriesController(IMapper mapper, IRoomCategoryService roomCategoryService) {
            _mapper = mapper;
            _roomCategoryService = roomCategoryService;
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
