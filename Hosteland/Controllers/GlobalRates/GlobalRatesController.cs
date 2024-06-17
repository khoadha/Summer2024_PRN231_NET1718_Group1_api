using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.GlobalRateService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers.GlobalRates
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalRatesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGlobalRateService _globalRateService;

        public GlobalRatesController(IMapper mapper, IGlobalRateService globalRateService)
        {
            _mapper = mapper;
            _globalRateService = globalRateService;
        }

        [HttpPost]
        [Route("add-rate")]
        public async Task<ActionResult<GlobalRate>> AddGlobalRate(AddGlobalRateDTO addServiceDto)
        {
            var cate = _mapper.Map<GlobalRate>(addServiceDto);

            var serviceResponse = await _globalRateService.AddGlobalRate(cate);
            var response = _mapper.Map<GlobalRateDTO>(serviceResponse.Data);

            return Ok(response);
        }
    }
}
