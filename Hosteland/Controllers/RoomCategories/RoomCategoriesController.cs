using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;
using Hosteland.Services.RoomCategoryService;
using Microsoft.AspNetCore.OData.Query;

namespace Hosteland.Controllers.RoomCategories
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomCategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoomCategoryService _roomCategoryService;

        public RoomCategoriesController(IMapper mapper, IRoomCategoryService roomCategoryService)
        {
            _mapper = mapper;
            _roomCategoryService = roomCategoryService;
        }

        [HttpGet]
        [Route("get-category")]
        public async Task<ActionResult<List<RoomCategory>>> GetRoomCategories()
        {
            var cates = await _roomCategoryService.GetRoomCategories();
            var resonse = _mapper.Map<List<GetRoomCategoryDto>>(cates.Data);
            return Ok(resonse);
        }

        [HttpPost]
        [Route("add-category")]
        public async Task<ActionResult<RoomCategory>> AddRoomCategory(AddRoomCategoryDto roomCategoryDto)
        {
            var cate = _mapper.Map<RoomCategory>(roomCategoryDto);

            var serviceResponse = await _roomCategoryService.AddRoomCategory(cate);
            var response = _mapper.Map<GetRoomCategoryDto>(serviceResponse.Data);

            return Ok(response);
        }

    }
}
