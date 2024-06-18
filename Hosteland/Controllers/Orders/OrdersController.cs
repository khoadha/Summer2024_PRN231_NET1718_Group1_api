using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.OrderService;
using Hosteland.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;


namespace Hosteland.Controllers.Orders
{
    [ApiController]
    [Route("api/v1/")]

    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IServiceService _serviceService;

        public OrdersController(IMapper mapper, IOrderService orderService, IServiceService serviceService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _serviceService = serviceService;
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
            // check model
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            // create list service contract 
            if (orderDto.RoomServices.Count > 0)
            {
                List<Contract> serviceContractList = new List<Contract>();
                var contractTypes = _orderService.GetContractTypes().Result.Data;

                // init contract type for 1st time
                if (contractTypes == null)
                {
                    AddContractTypeDto typeDto = new AddContractTypeDto();
                    typeDto.ContractName = "Room Contract";
                    var type = _mapper.Map<ContractType>(typeDto);

                    await _orderService.AddContractType(type);

                    AddContractTypeDto typeDto2 = new AddContractTypeDto();
                    typeDto.ContractName = "Service Contract";
                    var type2 = _mapper.Map<ContractType>(typeDto2);

                    await _orderService.AddContractType(type2);
                }

                // create contract for each service
                for (int i=0; i<orderDto.RoomServices.Count; i++)
                {
                    Contract serviceContract = new Contract();

                    serviceContract.ContractTypeId = contractTypes.FirstOrDefault(c => c.Id == 2).Id;
                    serviceContract.Type = contractTypes.FirstOrDefault(c => c.Id == 2);
                    var servId = orderDto.RoomServices[i].ServiceId;
                    //_serviceService.g
                    //serviceContract.Cost =

                    serviceContractList.Add(serviceContract);
                }           
            }

            // create 1 room contract
            var roomContract = _mapper.Map<CreateOrderDto, Contract>(orderDto);

            //create order 
            var order = _mapper.Map<CreateOrderDto, Order>(orderDto);

            var allOrders = _orderService.GetOrdersByRoomId(orderDto.RoomId).Result.Data;
            var overlapFlag = false;
            overlapFlag = allOrders.Any(order =>
                order.Contracts.Any(c => c.EndDate >= roomContract.StartDate && c.StartDate <= roomContract.StartDate)
            );
            if (overlapFlag)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() { "This room is already booked on that date." },
                    Result = false
                });
            }
            var createdOrder = await _orderService.CreateOrder(order, roomContract);
            return Ok(true);
        }

        [HttpPost]
        [Route("add-contracttype")]
        public async Task<ActionResult<RoomCategory>> AddContractType(AddContractTypeDto roomCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var cate = _mapper.Map<ContractType>(roomCategoryDto);

            var serviceResponse = await _orderService.AddContractType(cate);
            var response = _mapper.Map<GetContractTypeDto>(serviceResponse.Data);

            return Ok(response);
        }
    }
}
