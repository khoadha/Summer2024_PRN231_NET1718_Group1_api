using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using HostelandOData.Services.OrderService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HostelandOData.Controllers.Orders {

    [Route("odata/")]
    public class OOrdersController : ODataController {

        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OOrdersController(IMapper mapper, IOrderService orderService) {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet("OOrders")]
        [EnableQuery]
        public async Task<IActionResult> GetOrders() {
            var orders = await _orderService.GetOrders();
            var response = _mapper.Map<List<GetOrderDto>>(orders.Data);
            return Ok(response.AsQueryable());
        }
        [HttpGet("OOrders({id})")]
        [EnableQuery]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var rooms = await _orderService.GetOrderById(id);
            var response = _mapper.Map<GetOrderDto>(rooms.Data);
            return Ok(response);
        }

        [HttpGet("OOrders/ContractTypes")]
        [EnableQuery]
        public async Task<IActionResult> GetContractTypes()
        {
            var orders = await _orderService.GetContractTypes();
            var response = _mapper.Map<List<GetContractTypeDto>>(orders.Data);
            return Ok(response.AsQueryable());
        }
        
        [HttpGet("OOrders/FeeCategories")]
        [EnableQuery]
        public async Task<IActionResult> GetFeeCategories()
        {
            var orders = await _orderService.GetFeeCategories();
            var response = _mapper.Map<List<GetFeeCateDto>>(orders.Data);
            return Ok(response.AsQueryable());
        }
    }
}
