using AutoMapper;

using BusinessObjects.Constants;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.ReportService;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Hosteland.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {


        private readonly IMapper _mapper;
        private readonly IReportService _reportService;


        public ReportsController(IMapper mapper, IReportService reportService)
        {
            _mapper = mapper;
            _reportService = reportService;
        }


        [HttpPost]
        public async Task<IActionResult> AddReport(AddReportDto dto)
        {
            var rp = _mapper.Map<Report>(dto);
            var serviceResponse = await _reportService.AddReport(rp);
            var response = _mapper.Map<GetReportDto>(serviceResponse.Data);
            return Ok(response);
        }


        [HttpGet]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetReports()
        {
            var serviceResponse = await _reportService.GetReports();
            var response = _mapper.Map<List<GetReportDto>>(serviceResponse.Data);
            return Ok(response);
        }


        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetReportsByUserId([FromQuery] string userId)
        {
            var serviceResponse = await _reportService.GetReportsByUserId(userId);
            var response = _mapper.Map<List<GetReportDto>>(serviceResponse.Data);
            return Ok(response);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetReportById([FromRoute] int id)
        {
            var serviceResponse = await _reportService.GetReportById(id);
            var response = _mapper.Map<GetReportDto>(serviceResponse.Data);
            return Ok(response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateReport([FromRoute] int id, [FromBody] UpdateReportDto dto)
        {
            var report = _mapper.Map<Report>(dto);
            var serviceResponse = await _reportService.UpdateReport(id, report);
            var response = _mapper.Map<GetReportDto>(serviceResponse.Data);
            return Ok(response);
        }
    }
}
