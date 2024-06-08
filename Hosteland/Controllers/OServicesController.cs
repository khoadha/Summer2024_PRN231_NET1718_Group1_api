using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Drawing.Printing;

namespace Hosteland.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
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
        [EnableQuery(PageSize = 3)]
        public IQueryable<GetServiceDto> GetServices()
        {
            var cates = _serviceService.GetServices().Result.Data;
            var response = _mapper.Map<List<GetServiceDto>>(cates);
            return response.AsQueryable();
        }
    }
}
