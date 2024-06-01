using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using AutoMapper;
using HostelandAuthorization.Services.RoomService;
using HostelandAuthorization.Services.RoomCategoryService;
using BusinessObjects.DTOs;

namespace HostelandAuthorization.Controllers
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

        // GET: api/RoomCategories
        [HttpGet]
        [Route("get-category")]
        public async Task<ActionResult<List<RoomCategory>>> GetRoomCategories()
        {
            var cates = await _roomCategoryService.GetRoomCategories();
            var resonse = _mapper.Map<List<GetRoomCategoryDto>>(cates.Data);
            return Ok(resonse);
        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<RoomCategory>> GetRoomCategory(int id)
        //{
        //  if (_context.RoomCategories == null)
        //  {
        //      return NotFound();
        //  }
        //    var roomCategory = await _context.RoomCategories.FindAsync(id);

        //    if (roomCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return roomCategory;
        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutRoomCategory(int id, RoomCategory roomCategory)
        //{
        //    if (id != roomCategory.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(roomCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RoomCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPost]
        [Route("add-category")]
        public async Task<ActionResult<RoomCategory>> AddRoomCategory(AddRoomCategoryDto roomCategoryDto)
        {
            var cate = _mapper.Map<RoomCategory>(roomCategoryDto);

            var serviceResponse = await _roomCategoryService.AddRoomCategory(cate);
            var response = _mapper.Map<GetRoomCategoryDto>(serviceResponse.Data);

            return Ok(response);
        }

        //private bool RoomCategoryExists(int id)
        //{
        //    return (_context.RoomCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
