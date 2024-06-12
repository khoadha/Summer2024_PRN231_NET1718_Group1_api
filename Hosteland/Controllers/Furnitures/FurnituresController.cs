using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.FurnitureService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers.Furnitures
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
