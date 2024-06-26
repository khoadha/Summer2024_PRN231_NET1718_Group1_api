using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers.Services
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceService _serviceService;

        public ServicesController(IMapper mapper, IServiceService serviceService)
        {
            _mapper = mapper;
            _serviceService = serviceService;
        }

        [HttpPost("add-service")]
        public async Task<ActionResult<Service>> AddService([FromForm] AddServiceDto addServiceDto)
        {
            var cate = _mapper.Map<Service>(addServiceDto);

            var serviceResponse = await _serviceService.AddService(cate, addServiceDto.Image);
            var response = _mapper.Map<GetServiceDto>(serviceResponse.Data);

            return Ok(response);
        }

        [HttpPost]
        [Route("prices/add-price")]
        public async Task<ActionResult<Service>> AddServicePrice(AddServicePriceDto addServicePriceDto)
        {
            var cate = _mapper.Map<ServicePrice>(addServicePriceDto);

            var serviceResponse = await _serviceService.CreateServicePrice(cate);
            var response = _mapper.Map<ServicePriceDto>(serviceResponse.Data);

            return Ok(response);
        }


        [HttpGet]
        [Route("newest-price")]
        public async Task<IActionResult> GetServicesWithNewestPrice()
        {
            var cates = await _serviceService.GetServices();
            var response = _mapper.Map<List<GetServiceNewestPriceDto>>(cates.Data);
            return Ok(response.AsQueryable());
        }

    }
}
