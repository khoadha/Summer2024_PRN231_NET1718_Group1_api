using AutoMapper;
using BusinessObjects.DTOs;
using HostelandOData.Services.GlobalRateService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HostelandOData.Controllers.GlobalRates
{
    [Route("odata/")]
    public class OGlobalRatesController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly IGlobalRateService _globalRateService;

        public OGlobalRatesController(IMapper mapper, IGlobalRateService globalRateService)
        {
            _mapper = mapper;
            _globalRateService = globalRateService;
        }

        [HttpGet("OGlobalRates")]
        [EnableQuery]
        public async Task<IActionResult> GetRoomsDisplay()
        {
            var rooms = await _globalRateService.GetGlobalRates();
            var response = _mapper.Map<List<GlobalRateDTO>>(rooms.Data);
            return Ok(response.AsQueryable());
        }

        [HttpGet("OGlobalRates/NewestRate")]
        [EnableQuery]
        public async Task<IActionResult> GetServicesWithNewestPrice()
        {
            var cates = await _globalRateService.GetNewestGlobalRate();
            var response = _mapper.Map<GlobalRateDTO>(cates.Data);

            return Ok(response);
        }
    }
}
