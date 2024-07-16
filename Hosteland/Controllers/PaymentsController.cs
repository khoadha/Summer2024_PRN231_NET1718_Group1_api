using BusinessObjects.ConfigurationModels;
using BusinessObjects.Constants;
using BusinessObjects.Entities;
using Hosteland.Services.OrderService;
using Hosteland.Services.VnPayService;
using Hosteland.Context;
using Hosteland.Services.ApplicationUserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hosteland.Controllers.Payment
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IApplicationUserService _accountService;
        private readonly IUserContext _userContext;
        private readonly IOrderService _orderService;
        public PaymentsController(
            IUserContext userContext,
            IApplicationUserService accountService,
            IVnPayService vnPayService,
            IOrderService orderService)
        {
            _accountService = accountService;
            _vnPayService = vnPayService;
            _userContext = userContext;
            _orderService = orderService;
        }

        [HttpGet("transactions")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int? count)
        {

            var data = await _vnPayService.GetPaymentTransactions(count);
            return Ok(data.Data);
        }

        [HttpGet("top-transactions")]
        //[Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetLatestTransactions()
        {

            var data = await _vnPayService.GetTopLatestTransactions();
            return Ok(data.Data);
        }

        [HttpGet("transactions-data")]
        //[Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllTransactionsFeeAndDate([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {

            var data = await _vnPayService.GetTransactionAmountsAndDates(fromDate,toDate);
            return Ok(data.Data);
        }

        [HttpGet("total-transactions")]
        //[Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetTotalTransactions()
        {

            var data = await _vnPayService.GetTotalTransactionCount();
            return Ok(data.Data);
        }

        [HttpGet("transactions/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetTransactionsByUserId([FromRoute] string? userId) {
            var currentUser = _userContext.GetCurrentUser(HttpContext);

            if (currentUser.UserId != userId)
            {
                return Forbid();
            }

            var data = await _vnPayService.GetTransactionsByUserId(userId);
            return Ok(data.Data);
        }

        [HttpPost("create/{userId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentRequestModel paymentRequestModel, [FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Result = false, Message = "Invalid data" });
            }
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResult
                {
                    Result = false,
                    Errors = new List<string> { "Please sign in." }
                });
            }
            var requestUser = _userContext.GetCurrentUser(HttpContext);
            //if (requestUser.UserId != userId)
            //{
            //    return Forbid();
            //}

            var host = Request.Headers.Referer;

            if (paymentRequestModel.FeeIds.Count <= 0)
            {
                return BadRequest("Should have fee");
            }
            var allFee = new List<Fee>();

            for (var i = 0; i < paymentRequestModel.FeeIds.Count; i++)
            {
                try
                { 
                    var fee = _orderService.GetFeeById(paymentRequestModel.FeeIds[i]).Result.Data;
                    allFee.Add(fee);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            if (allFee == null || allFee.Count != paymentRequestModel.FeeIds.Count)
            {
                return BadRequest("One or more fees are invalid.");
            }

            // Calculate total amount
            var totalAmount = allFee.Sum(f => f.Amount);

            var paymentModel = new PaymentInformationModel
            {
                OrderType = "Pay Fees",
                Amount = totalAmount,
                OrderDescription = paymentRequestModel.Description ?? string.Empty,
                Name = "",
            };

            var url = await _vnPayService.CreatePaymentUrl(paymentModel, userId, HttpContext, host, allFee);
            var response = new { url };

            return Ok(response);
        }

        [HttpPut("payment-success/{txnRef}/{userId}")]
        [Authorize]
        public IActionResult HandlePaymentSuccess([FromRoute] string txnRef, [FromRoute] string userId)
        {
            //txnRef = transactionId
            var requestUser = _userContext.GetCurrentUser(HttpContext);

            if (requestUser.UserId != userId)
            {
                return Forbid();
            }
            else
            {
                _vnPayService.HandlePaymentSuccess(txnRef);
            }

            return NoContent();
        }
    }
}
