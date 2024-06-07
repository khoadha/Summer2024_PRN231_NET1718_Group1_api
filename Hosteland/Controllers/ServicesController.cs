using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers
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

        [HttpGet]
        [Route("get-service")]
        public async Task<ActionResult<List<Service>>> GetServices()
        {
            var cates = await _serviceService.GetServices();
            var response = _mapper.Map<List<GetServiceDto>>(cates.Data);
            return Ok(response);
        }


        [HttpGet]
        [Route("get-service-newest-price")]
        public async Task<ActionResult<List<Service>>> GetServicesWithNewestPrice()
        {
            var cates = await _serviceService.GetServices();
            var response = _mapper.Map<List<GetServiceNewewstPriceDto>>(cates.Data);
            return Ok(response);
        }
        [HttpGet]
        [Route("prices/get-price/{serviceId}")]
        public async Task<ActionResult<List<ServicePrice>>> GetServicePricesByServiceId([FromRoute] int serviceId)
        {
            var cates = await _serviceService.GetServicePricesByServiceId(serviceId);
            var response = _mapper.Map<List<ServicePriceDto>>(cates.Data);
            return Ok(response);
        }

        [HttpPost]
        [Route("add-service")]
        public async Task<ActionResult<Service>> AddService(AddServiceDto addServiceDto)
        {
            var cate = _mapper.Map<Service>(addServiceDto);

            var serviceResponse = await _serviceService.AddService(cate);
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
    }
}
