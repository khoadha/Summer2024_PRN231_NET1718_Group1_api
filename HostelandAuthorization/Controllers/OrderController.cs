using AutoMapper;
using BusinessObjects.Entities;
using HostelandAuthorization.Services.OrderService;
using HostelandAuthorization.Services.OrderService;
using HostelandAuthorization.Shared;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostelandAuthorization.Controllers
{
    [ApiController]
    [Route("api/v1/")]

    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController (IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("order/get-order")]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            return Ok(orders);
        }

        [HttpPost]
        [Route("order/add")]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var createdOrder = await _orderService.AddOrder(order);
            return Ok(createdOrder);
        }
        [HttpPost]
        [Route("order/create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var order = _mapper.Map<CreateOrderDto, Order>(orderDto);
            var contract = _mapper.Map<CreateOrderDto, Contract>(orderDto);

            var createdOrder = await _orderService.CreateOrder(order,contract);
            return Ok(createdOrder);
        }
    }
}
