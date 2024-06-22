using AutoMapper;
using BusinessObjects.DTOs;
using HostelandOData.Services.FurnitureService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HostelandOData.Controllers.Furnitures {

    [Route("odata/")]
    public class OFurnituresController : ODataController {
        private readonly IMapper _mapper;
        private readonly IFurnitureService _furnitureService;

        public OFurnituresController(IMapper mapper, IFurnitureService furnitureService) {
            _mapper = mapper;
            _furnitureService = furnitureService;
        }

        [HttpGet("OFurnitures")]
        [EnableQuery]
        public async Task<IActionResult> GetFurnitures() {
            var furn = await _furnitureService.GetFurnitures();
            var response = _mapper.Map<List<FurnitureDTO>>(furn.Data);
            return Ok(response.AsQueryable());
        }
    }
}
