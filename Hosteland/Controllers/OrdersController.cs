using AutoMapper;
using BusinessObjects.ConfigurationModels;
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Hosteland.Services.GlobalRateService;
using Hosteland.Services.OrderService;
using Hosteland.Services.RoomService;
using Hosteland.Services.ServiceService;
using Hosteland.Context;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using BusinessObjects.Constants;

namespace Hosteland.Controllers.Orders {
    [ApiController]
    [Route("api/v1/")]

    public class OrdersController : ControllerBase {
        #region dependency injection
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IServiceService _serviceService;
        private readonly IRoomService _roomService;
        private readonly IGlobalRateService _globalRateService;
        private readonly IUserContext _userContext;

        public OrdersController(
            IMapper mapper,
            IOrderService orderService,
            IServiceService serviceService,
            IRoomService roomService,
            IGlobalRateService globalRateService,
            IUserContext userContext) {
            _mapper = mapper;
            _orderService = orderService;
            _serviceService = serviceService;
            _roomService = roomService;
            _globalRateService = globalRateService;
            _userContext = userContext;
        }
        #endregion

        [HttpPost]
        [Route("order/add")]
        [Authorize]
        public async Task<IActionResult> AddOrder([FromBody] Order order) {
            if (!ModelState.IsValid) {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var createdOrder = await _orderService.AddOrder(order);
            return Ok(createdOrder);
        }

        [HttpGet("order/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById([FromRoute] int id) {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            var order = await _orderService.GetOrderById(id);

            if (currentUser.UserId != order.Data.UserId) {
                return Forbid();
            }

            var response = _mapper.Map<GetOrderDto>(order.Data);

            return Ok(response);
        }

        [HttpGet("order/get-user-id/{id}")]
        public async Task<IActionResult> GetOrderByUserId([FromRoute] string id) {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if (currentUser.UserId != id) {
                return Forbid();
            }

            var order = await _orderService.GetOrders();

            var response = _mapper.Map<List<GetOrderDto>>(order.Data.Where(o => o.UserId == id));

            return Ok(response);
        }
        [HttpGet]
        [Route("order/get-fee/{orderId}")]
        [Authorize]
        public async Task<ActionResult<List<Fee>>> GetFeesByOrderId([FromRoute] int orderId, string userId) {


            var requestUser = _userContext.GetCurrentUser(HttpContext);
            if (requestUser == null || requestUser.UserId != userId) {
                return Forbid();
            }
            var list = await _orderService.GetFeesByOrderId(orderId);
            var res = _mapper.Map<List<GetFeeDto>>(list.Data);
            return Ok(res);
        }

        [HttpGet("order/deferred-electricity-fee")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetDeferredElectricityFees() {
            var list = await _orderService.GetDeferredElectricityFee();
            var res = _mapper.Map<List<GetDeferredElectricityFeeDto>>(list.Data);
            return Ok(res);
        }

        [HttpPost("order/update-amount-fee")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> UpdateAmountFee([FromBody] UpdateAmountFeeRequestDTO dto) {

            if (dto is not null) {
                await _orderService.UpdateAmountFee(dto);
            }

            var list = await _orderService.GetDeferredElectricityFee();
            var res = _mapper.Map<List<GetDeferredElectricityFeeDto>>(list.Data);
            return Ok(res);
        }

        [HttpPost("order")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto) {

            var today = DateTime.Now;

            // check model
            if (!ModelState.IsValid) {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            // init CONTRACT and FEE type for 1st time
            await InitContractType();
            await InitFeeCate();
            var feeCates = _orderService.GetFeeCates().Result.Data;
            var contractTypes = _orderService.GetContractTypes().Result.Data;
            var dayOccupied = (orderDto.EndDate - orderDto.StartDate).Value.TotalDays + 1;
            var guestsCount = orderDto.Guests.Count;

            double dateDiffAdditionalFee = 0;
            var startDate = orderDto.StartDate;

            List<Contract> allContract = new List<Contract>();
            List<Fee> allFee = new List<Fee>();

            // SERVICE 's contract and fee
            if (orderDto.RoomServices.Count > 0) {
                for (int i = 0; i < orderDto.RoomServices.Count; i++) 
                {
                    // CONTRACT
                    var serviceId = orderDto.RoomServices[i].ServiceId;
                    var bookedService = _serviceService.GetServiceById(serviceId).Result.Data;
                    var bookedServiceNewestPrice = _serviceService.GetServiceNewestPricesByServiceId(serviceId).Result.Data;
                    var bookedServiceContractPrice = _serviceService.GetServicePricesInContractByServiceId(serviceId, orderDto.StartDate).Result.Data;
                    double newestServiceCost = 0;
                    double thatTimeServiceCost = 0;  // AKA cost of serv in Contract at that time
                    if (bookedServiceNewestPrice != null) {
                        newestServiceCost = bookedServiceNewestPrice.Amount;
                    }
                    if (bookedServiceContractPrice != null)
                    {
                        thatTimeServiceCost = bookedServiceContractPrice.Amount;
                    }

                    Contract serviceContract = new Contract();

                    serviceContract = _mapper.Map<CreateOrderDto, Contract>(orderDto);

                    serviceContract.ContractTypeId = contractTypes.FirstOrDefault(c => c.Id == 2).Id;
                    serviceContract.Type = contractTypes.FirstOrDefault(c => c.Id == 2);
                    if(bookedService.IsCountPerCapita == true) {
                        serviceContract.Cost = thatTimeServiceCost * dayOccupied * guestsCount;
                    } else {
                        serviceContract.Cost = thatTimeServiceCost * dayOccupied;
                    }
                    serviceContract.Name = bookedService.Name + " Service Contract";
                    serviceContract.ServicePriceId = bookedServiceNewestPrice.Id;
                    allContract.Add(serviceContract);

                    // FEE
                    if (!orderDto.IsMonthly) {
                        Fee serviceFee = new Fee();
                        serviceFee.FeeCategoryId = feeCates.FirstOrDefault(c => c.Id == 2).Id;
                        serviceFee.FeeCategory = feeCates.FirstOrDefault(c => c.Id == 2);
                        serviceFee.FeeStatus = FeeStatus.Unpaid;
                        serviceFee.PaymentDate = orderDto.StartDate;

                        if (bookedService != null) {
                            if (bookedService.IsCountPerCapita is true) {
                                serviceFee.Amount = thatTimeServiceCost * guestsCount * dayOccupied;
                            } else {
                                serviceFee.Amount = thatTimeServiceCost * dayOccupied;
                            }
                            serviceFee.Name = bookedService.Name + " Fee";
                        }
                        allFee.Add(serviceFee);
                    } else {
                        double serviceAmount = 0;
                        if (bookedService != null) {
                            if (bookedService.IsCountPerCapita is true) {
                                serviceAmount = thatTimeServiceCost * guestsCount * dayOccupied;
                            } else {
                                serviceAmount = thatTimeServiceCost * dayOccupied;
                            }
                        }
                        dateDiffAdditionalFee += CalculateDayDifferenceFromBillingDay((DateTime)startDate) * serviceAmount;
                    }
                }
            }


            // ROOM 's contract and fee
            var bookedRoom = _roomService.GetRoomById(orderDto.RoomId).Result.Data;
            var roomCost = _roomService.GetRoomById(orderDto.RoomId).Result.Data.CostPerDay;
            Contract roomContract = new Contract();

            // CONTRACT
            roomContract = _mapper.Map<CreateOrderDto, Contract>(orderDto);
            roomContract.Name = $"Room {bookedRoom.Name} Rental Contract";
            roomContract.ContractTypeId = contractTypes.FirstOrDefault(c => c.Id == 1).Id;
            roomContract.Type = contractTypes.FirstOrDefault(c => c.Id == 1);
            if(!orderDto.IsMonthly)
            {
                roomContract.Cost = roomCost * dayOccupied;
            }
            else
            {
                roomContract.Cost = roomCost * 0.9 * dayOccupied;
            }
            allContract.Add(roomContract);

            // FEE
            if (!orderDto.IsMonthly) {
                Fee roomFee = new Fee();
                roomFee.FeeCategoryId = feeCates.FirstOrDefault(c => c.Id == 1).Id;
                roomFee.FeeCategory = feeCates.FirstOrDefault(c => c.Id == 1);
                roomFee.FeeStatus = FeeStatus.Unpaid;
                roomFee.PaymentDate = orderDto.StartDate;
                roomFee.Amount = roomCost * dayOccupied;
                roomFee.Name = "Room Fees";
                allFee.Add(roomFee);
            } else {
                if (startDate is not null) {
                    dateDiffAdditionalFee += CalculateDayDifferenceFromBillingDay((DateTime)startDate) * roomCost;
                }
            }

            // create FEE deposit
            var globalServiceRes = await _globalRateService.GetNewestGlobalRate();
            var rate = globalServiceRes.Data;
            if (!globalServiceRes.Success || rate == null) {
                return NotFound("Rate not found");
            }
            var totalCost = roomCost;
            foreach (var fee in allFee) {
                totalCost += fee.Amount;
            }

            Fee depositFee = new Fee();
            depositFee.Name = "Deposit Fee";
            depositFee.FeeCategoryId = feeCates.FirstOrDefault(c => c.Id == 3).Id;
            depositFee.FeeCategory = feeCates.FirstOrDefault(c => c.Id == 3);
            depositFee.FeeStatus = FeeStatus.Unpaid;
            depositFee.PaymentDate = orderDto.StartDate;
            double depositRate = (double)rate.Deposit == null ? 0 : (double)rate.Deposit;
            depositFee.Amount = (double)(totalCost * rate.Deposit);

            if (dateDiffAdditionalFee != 0) {
                depositFee.Amount -= dateDiffAdditionalFee;
            }

            allFee.Add(depositFee);

            //create order 
            var order = _mapper.Map<CreateOrderDto, Order>(orderDto);

            var orderServiceRes = _orderService.GetOrdersByRoomId(orderDto.RoomId).Result;
            if (!orderServiceRes.Success) {
                return BadRequest("Order not found");
            }
            var allOrders = _orderService.GetOrdersByRoomId(orderDto.RoomId).Result.Data;
            var overlapFlag = false;
            overlapFlag = allOrders.Any(order =>
                 order.Contracts.Any(c => c.EndDate >= roomContract.StartDate && c.StartDate <= roomContract.EndDate)
            );
            if (overlapFlag) {
                return BadRequest(new AuthResult() {
                    Errors = new List<string>() { "This room is already booked on that date." },
                    Result = false
                });
            }
            var createdOrder = await _orderService.CreateOrder(order, allContract, allFee);

            return Ok(true);
        }

        [HttpPost]
        [Route("add-contracttype")]
        public async Task<ActionResult<RoomCategory>> AddContractType(AddContractTypeDto roomCategoryDto) {
            if (!ModelState.IsValid) {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }

            var cate = _mapper.Map<ContractType>(roomCategoryDto);

            var serviceResponse = await _orderService.AddContractType(cate);
            var response = _mapper.Map<GetContractTypeDto>(serviceResponse.Data);

            return Ok(response);
        }

        #region Private method 
        private async Task InitContractType() {
            var serviceRes = _orderService.GetContractTypes().Result;
            var contractTypes = serviceRes.Data;

            if (!contractTypes.Any() && serviceRes.Success) {
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

        private async Task InitFeeCate() {
            var serviceRes = _orderService.GetFeeCates().Result;
            var cates = serviceRes.Data;
            if (!cates.Any() && serviceRes.Success) {
                AddFeeCateDto typeDto1 = new AddFeeCateDto();
                typeDto1.FeeCategoryName = "Room Fee";
                var type1 = _mapper.Map<FeeCategory>(typeDto1);
                await _orderService.AddFeeCate(type1);

                AddFeeCateDto typeDto2 = new AddFeeCateDto();
                typeDto2.FeeCategoryName = "Service Fee";
                var type2 = _mapper.Map<FeeCategory>(typeDto2);
                await _orderService.AddFeeCate(type2);

                AddFeeCateDto typeDto3 = new AddFeeCateDto();
                typeDto3.FeeCategoryName = "Deposit Fee";
                var type3 = _mapper.Map<FeeCategory>(typeDto3);
                await _orderService.AddFeeCate(type3);

                AddFeeCateDto typeDto4 = new AddFeeCateDto();
                typeDto4.FeeCategoryName = "Furniture Fee";
                var type4 = _mapper.Map<FeeCategory>(typeDto4);
                await _orderService.AddFeeCate(type4);

                AddFeeCateDto typeDto5 = new AddFeeCateDto();
                typeDto5.FeeCategoryName = "Water Fee";
                var type5 = _mapper.Map<FeeCategory>(typeDto5);
                await _orderService.AddFeeCate(type5);

                AddFeeCateDto typeDto6 = new AddFeeCateDto();
                typeDto6.FeeCategoryName = "Electric Fee";
                var type6 = _mapper.Map<FeeCategory>(typeDto6);
                await _orderService.AddFeeCate(type6);

                AddFeeCateDto typeDto7 = new AddFeeCateDto();
                typeDto7.FeeCategoryName = "Fine";
                var type7 = _mapper.Map<FeeCategory>(typeDto7);
                await _orderService.AddFeeCate(type7);
            }
        }

        private static int CalculateDayDifferenceFromBillingDay(DateTime inputDate) {
            DateTime fifteenthDate = new DateTime(inputDate.Year, inputDate.Month, 15);
            int dayDifference = (fifteenthDate - inputDate).Days;
            return dayDifference;
        }

        #endregion
    }


}
