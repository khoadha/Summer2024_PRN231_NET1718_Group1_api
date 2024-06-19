using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Hosteland.Services.OrderService;
using Hosteland.Services.RoomService;
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
        private readonly IRoomService _roomService;

        public OrdersController(IMapper mapper, IOrderService orderService, IServiceService serviceService, IRoomService roomService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _serviceService = serviceService;
            _roomService = roomService;
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

            // init contract type for 1st time
            var contractTypes = _orderService.GetContractTypes().Result.Data;
            if (!contractTypes.Any())
            {
                AddContractTypeDto typeDto = new AddContractTypeDto();
                typeDto.ContractName = "Room Contract";
                var type = _mapper.Map<ContractType>(typeDto);

                await _orderService.AddContractType(type);

                AddContractTypeDto typeDto2 = new AddContractTypeDto();
                typeDto2.ContractName = "Service Contract";
                var type2 = _mapper.Map<ContractType>(typeDto2);

                await _orderService.AddContractType(type2);
            }

            List<Contract> allContract = new List<Contract>();

            // create list service contract 
            if (orderDto.RoomServices.Count > 0)
            {
                // create contract for each service
                for (int i=0; i<orderDto.RoomServices.Count; i++)
                {
                    Contract serviceContract = new Contract();

                    // prop for contract
                    serviceContract = _mapper.Map<CreateOrderDto, Contract>(orderDto);

                    serviceContract.ContractTypeId = contractTypes.FirstOrDefault(c => c.Id == 2).Id;
                    serviceContract.Type = contractTypes.FirstOrDefault(c => c.Id == 2);
                    var serviceId = orderDto.RoomServices[i].ServiceId;                  
                    serviceContract.Cost = _serviceService.GetServiceNewestPricesByServiceId(serviceId).Result.Data.Amount;

                    allContract.Add(serviceContract);
                }           
            }

            // create 1 room contract
            Contract roomContract = new Contract();

            roomContract = _mapper.Map<CreateOrderDto, Contract>(orderDto);
            roomContract.ContractTypeId = contractTypes.FirstOrDefault(c => c.Id == 1).Id;
            roomContract.Type = contractTypes.FirstOrDefault(c => c.Id == 1);
            var roomCost = _roomService.GetRoomById(orderDto.RoomId).Result.Data.CostPerDay;
            TimeSpan? v = (roomContract.EndDate - roomContract.StartDate);
            roomContract.Cost = roomCost * v.Value.TotalDays;

            allContract.Add(roomContract);

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
            var createdOrder = await _orderService.CreateOrder(order, allContract);
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
