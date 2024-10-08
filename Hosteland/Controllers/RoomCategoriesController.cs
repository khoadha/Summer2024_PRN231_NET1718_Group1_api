﻿using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;
using Hosteland.Services.RoomCategoryService;
using Microsoft.AspNetCore.Authorization;
using BusinessObjects.Constants;

namespace Hosteland.Controllers.Rooms
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

        [HttpPost]
        [Route("add-category")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<ActionResult<RoomCategory>> AddRoomCategory(AddRoomCategoryDto roomCategoryDto)
        {
            var cate = _mapper.Map<RoomCategory>(roomCategoryDto);

            var serviceResponse = await _roomCategoryService.AddRoomCategory(cate);
            var response = _mapper.Map<GetRoomCategoryDto>(serviceResponse.Data);

            return Ok(response);
        }

    }
}
