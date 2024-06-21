using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Hosteland.Services.GlobalRateService;
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
        private readonly IGlobalRateService _globalRateService;

        public OrdersController(
            IMapper mapper,
            IOrderService orderService,
            IServiceService serviceService,
            IRoomService roomService,
            IGlobalRateService globalRateService)
        {
            _mapper = mapper;
            _orderService = orderService;
            _serviceService = serviceService;
            _roomService = roomService;
            _globalRateService = globalRateService;
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

        [HttpGet]
        [Route("order/get-fee/{id}")]
        public async Task<ActionResult<List<Fee>>> GetFeesByOrderId([FromRoute] int orderId)
        {
            var list = await _orderService.GetFeesByOrderId(orderId);
            var res = _mapper.Map<GetFeeDto>(list);
            return Ok(res);
        }

        [HttpPost]
        [Route("order/create-day")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            // check model
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            // init CONTRACT and FEE type for 1st time
            await InitContractType();
            await InitFeeCate();
            var feeCates = _orderService.GetFeeCates().Result.Data;
            var contractTypes = _orderService.GetContractTypes().Result.Data;
            var dayOccupied = (orderDto.EndDate - orderDto.StartDate).Value.TotalDays;

            List<Contract> allContract = new List<Contract>();
            List<Fee> allFee = new List<Fee>();

            // create list service CONTRACT and FEE 
            if (orderDto.RoomServices.Count > 0)
            {
                // create contract for each service
                for (int i = 0; i < orderDto.RoomServices.Count; i++)
                {
                    var serviceId = orderDto.RoomServices[i].ServiceId;
                    var serviceCost = _serviceService.GetServiceNewestPricesByServiceId(serviceId).Result.Data.Amount;

                    Contract serviceContract = new Contract();
                    Fee serviceFee = new Fee();

                    // prop for contract
                    serviceContract = _mapper.Map<CreateOrderDto, Contract>(orderDto);

                    serviceContract.ContractTypeId = contractTypes.FirstOrDefault(c => c.Id == 2).Id;
                    serviceContract.Type = contractTypes.FirstOrDefault(c => c.Id == 2);
                    serviceContract.Cost = serviceCost * dayOccupied;

                    allContract.Add(serviceContract);

                    // prop for fee
                    serviceFee.FeeCategoryId = feeCates.FirstOrDefault(c => c.Id == 2).Id;
                    serviceFee.FeeCategory = feeCates.FirstOrDefault(c => c.Id == 2);
                    serviceFee.FeeStatus = FeeStatus.Unpaid;
                    serviceFee.PaymentDate = orderDto.StartDate;
                    serviceFee.Amount = serviceCost * dayOccupied;

                    allFee.Add(serviceFee);
                }
            }

            // create 1 room CONTRACT and FEE
            var roomCost = _roomService.GetRoomById(orderDto.RoomId).Result.Data.CostPerDay;

            Contract roomContract = new Contract();
            Fee roomFee = new Fee();

            roomContract = _mapper.Map<CreateOrderDto, Contract>(orderDto);
            roomContract.ContractTypeId = contractTypes.FirstOrDefault(c => c.Id == 1).Id;
            roomContract.Type = contractTypes.FirstOrDefault(c => c.Id == 1);
            roomContract.Cost = roomCost * dayOccupied;

            allContract.Add(roomContract);

            roomFee.FeeCategoryId = feeCates.FirstOrDefault(c => c.Id == 1).Id;
            roomFee.FeeCategory = feeCates.FirstOrDefault(c => c.Id == 1);
            roomFee.FeeStatus = FeeStatus.Unpaid;
            roomFee.PaymentDate = orderDto.StartDate;
            roomFee.Amount = roomCost * dayOccupied;

            allFee.Add(roomFee);

            // create FEE deposit
            var globalServiceRes = _globalRateService.GetNewestGlobalRate().Result;
            if (!globalServiceRes.Success)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() {
                        "Internal Server Error"
                    },
                    Result = false
                });
            }
            var rate = globalServiceRes.Data;
            var totalCost = roomCost;
            foreach (var fee in allFee)
            {
                totalCost += fee.Amount;
            }

            Fee depositFee = new Fee();
            depositFee.FeeCategoryId = feeCates.FirstOrDefault(c => c.Id == 3).Id;
            depositFee.FeeCategory = feeCates.FirstOrDefault(c => c.Id == 3);
            depositFee.FeeStatus = FeeStatus.Unpaid;
            depositFee.PaymentDate = orderDto.StartDate;
            depositFee.Amount = (double)(totalCost * rate.Deposit);

            allFee.Add(depositFee);

            //create order 
            var order = _mapper.Map<CreateOrderDto, Order>(orderDto);

            var orderServiceRes = _orderService.GetOrdersByRoomId(orderDto.RoomId).Result;
            if (!orderServiceRes.Success)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() {
                        "Internal Server Error"
                    },
                    Result = false
                });
            }
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
            var createdOrder = await _orderService.CreateOrder(order, allContract, allFee);

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

        private async Task InitContractType()
        {
            var serviceRes = _orderService.GetContractTypes().Result;
            var contractTypes = serviceRes.Data;

            if (!contractTypes.Any() && serviceRes.Success)
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
        }

        private async Task InitFeeCate()
        {
            var serviceRes = _orderService.GetFeeCates().Result;
            var cates = serviceRes.Data;
            if (!cates.Any() && serviceRes.Success)
            {
                AddFeeCateDto typeDto1 = new AddFeeCateDto();
                typeDto1.FeeName = "Room Fee";
                var type1 = _mapper.Map<FeeCategory>(typeDto1);
                await _orderService.AddFeeCate(type1);

                AddFeeCateDto typeDto2 = new AddFeeCateDto();
                typeDto2.FeeName = "Service Fee";
                var type2 = _mapper.Map<FeeCategory>(typeDto2);
                await _orderService.AddFeeCate(type2);

                AddFeeCateDto typeDto3 = new AddFeeCateDto();
                typeDto3.FeeName = "Deposit Fee";
                var type3 = _mapper.Map<FeeCategory>(typeDto3);
                await _orderService.AddFeeCate(type3);

                AddFeeCateDto typeDto4 = new AddFeeCateDto();
                typeDto4.FeeName = "Furniture Fee";
                var type4 = _mapper.Map<FeeCategory>(typeDto4);
                await _orderService.AddFeeCate(type4);

                AddFeeCateDto typeDto5 = new AddFeeCateDto();
                typeDto5.FeeName = "Water Fee";
                var type5 = _mapper.Map<FeeCategory>(typeDto5);
                await _orderService.AddFeeCate(type5);
                
                AddFeeCateDto typeDto6 = new AddFeeCateDto();
                typeDto6.FeeName = "Electric Fee";
                var type6 = _mapper.Map<FeeCategory>(typeDto6);
                await _orderService.AddFeeCate(type6);
                
                AddFeeCateDto typeDto7 = new AddFeeCateDto();
                typeDto7.FeeName = "Fine";
                var type7 = _mapper.Map<FeeCategory>(typeDto7);
                await _orderService.AddFeeCate(type7);
            }
        }
    }
}
