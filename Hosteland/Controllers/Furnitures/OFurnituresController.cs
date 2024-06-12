﻿using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.FurnitureService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;


namespace Hosteland.Controllers.Furnitures {

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
        public async Task<ActionResult<List<RoomCategory>>> GetFurnitures() {
            var furn = await _furnitureService.GetFurnitures();
            var response = _mapper.Map<List<FurnitureDTO>>(furn.Data);
            return Ok(response.AsQueryable());
        }

        [HttpGet("OFurnitures({id})")]
        [EnableQuery]
        public async Task<ActionResult<List<RoomCategory>>> GetFurnitures([FromRoute] int id) {
            var furns = await _furnitureService.GetFurnitures();
            var furn = furns.Data.FirstOrDefault(a=>a.Id==id);
            var response = _mapper.Map<FurnitureDTO>(furn);
            return Ok(response);
        }
    }
}
