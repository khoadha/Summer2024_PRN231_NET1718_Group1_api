using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using HostelandAuthorization.Services.FurnitureService;
using HostelandAuthorization.Services.RoomCategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelandAuthorization.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FurnituresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFurnitureService _furnitureService;

        public FurnituresController(IMapper mapper, IFurnitureService furnitureService)
        {
            _mapper = mapper;
            _furnitureService = furnitureService;
        }

        [HttpGet]
        [Route("get-furniture")]
        public async Task<ActionResult<List<RoomCategory>>> GetFurnitures()
        {
            var furn = await _furnitureService.GetFurnitures();
            var resonse = _mapper.Map<List<FurnitureDTO>>(furn.Data);
            return Ok(resonse);
        }

        [HttpPost]
        [Route("add-furniture")]
        public async Task<ActionResult<RoomCategory>> AddFurniture(AddFurnitureDTO dto)
        {
            var cate = _mapper.Map<Furniture>(dto);

            var serviceResponse = await _furnitureService.AddFurniture(cate);
            var response = _mapper.Map<FurnitureDTO>(serviceResponse.Data);

            return Ok(response);
        }
    }
}
