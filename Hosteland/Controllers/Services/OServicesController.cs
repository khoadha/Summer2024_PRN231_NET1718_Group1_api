using AutoMapper;
using BusinessObjects.DTOs;
using Hosteland.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Hosteland.Controllers.OServices
{
    [Route("odata/ServicesController")]
    public class OServicesController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly IServiceService _serviceService;

        public OServicesController(IMapper mapper, IServiceService serviceService)
        {
            _mapper = mapper;
            _serviceService = serviceService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetServices()
        {
            var cates = await _serviceService.GetServices();
            var response = _mapper.Map<List<GetServiceDto>>(cates.Data);
            return Ok(response.AsQueryable());
        }
    }
}
