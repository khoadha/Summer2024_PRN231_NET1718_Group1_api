using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.OrderService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hosteland.Controllers
{
    [ApiController]
    [Route("api/v1/")]

    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrdersController (IMapper mapper, IOrderService orderService)
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

        [HttpGet]
        [Route("order/get-order/{id}")]
        public async Task<ActionResult<Order>> GetOrderById([FromRoute] int id)
        {
            var orders = await _orderService.GetOrderById(id);
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

            var allOrders = _orderService.GetOrdersByRoomId(orderDto.RoomId).Result.Data;
            var overlapFlag = false;
            overlapFlag = allOrders.Any(order =>
                order.Contracts.Any(c => c.EndDate >= contract.StartDate && c.StartDate <= contract.StartDate)
            );
            if (overlapFlag)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() { "This room is already booked on that date." },
                    Result = false
                });
            }
            var createdOrder = await _orderService.CreateOrder(order,contract);
            return Ok(createdOrder);
        }
    }
}
