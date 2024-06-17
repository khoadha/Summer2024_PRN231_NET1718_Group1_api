using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Hosteland.Controllers.Services {

    [Route("odata/")]
    public class OServicesController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly IServiceService _serviceService;

        public OServicesController(IMapper mapper, IServiceService serviceService) {
            _mapper = mapper;
            _serviceService = serviceService;
        }

        [HttpGet("OServices")]
        [EnableQuery]
        public async Task<IActionResult> GetServices() {
            var cates = await _serviceService.GetServices();
            var response = _mapper.Map<List<GetServiceDto>>(cates.Data);
            return Ok(response.AsQueryable());
        }


        [HttpGet("OServices/NewestPrice")]
        [EnableQuery]
        public async Task<IActionResult> GetServicesWithNewestPrice() {
            var cates = await _serviceService.GetServices();
            var response = _mapper.Map<List<GetServiceNewestPriceDto>>(cates.Data);
            return Ok(response.AsQueryable());
        }

        [HttpGet("OServicePrices({serviceId})")]
        [EnableQuery]
        public async Task<IActionResult> GetServicePricesByServiceId([FromRoute] int serviceId) {
            var cates = await _serviceService.GetServicePricesByServiceId(serviceId);
            var response = _mapper.Map<List<ServicePriceDto>>(cates.Data);
            return Ok(response.AsQueryable());
        }
    }
}
