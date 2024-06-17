using BusinessObjects.ConfigurationModels;
using BusinessObjects.Constants;
using Hosteland.Context;
using Hosteland.Services.ApplicationUserService;
using Hosteland.Services.VnPayService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public PaymentsController(
            IUserContext userContext,
            IApplicationUserService accountService,
            IVnPayService vnPayService)
        {
            _accountService = accountService;
            _vnPayService = vnPayService;
            _userContext = userContext;
        }

        [HttpGet("transactions")]
        [Authorize(Roles = AppRole.ADMIN)]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int? count)
        {

            var data = await _vnPayService.GetPaymentTransactions(count);
            return Ok(data.Data);
        }

        [HttpPost("create/{userId}")]
        [Authorize]
        public IActionResult CreatePaymentUrl([FromBody] PaymentRequestModel paymentRequestModel, [FromRoute] string userId)
        {

            var requestUser = _userContext.GetCurrentUser(HttpContext);

            var host = Request.Headers.Referer;

            if (requestUser.UserId != userId)
            {
                return Forbid();
            }

            var paymentModel = new PaymentInformationModel
            {
                OrderType = "Fee",
                Amount = paymentRequestModel.PaymentAmount,
                OrderDescription = paymentRequestModel.Description,
                Name = "",
            };

            var url = _vnPayService.CreatePaymentUrl(paymentModel, userId, HttpContext, host);
            var response = new { url = url.Result };

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

        //[HttpGet("point")]
        //[Authorize]
        //public async Task<IActionResult> GetWemadePoint(string userId)
        //{

        //    var requestUser = _userContext.GetCurrentUser(HttpContext);

        //    if (requestUser.UserId != userId)
        //    {
        //        return Forbid();
        //    }

        //    var response = await _accountService.GetWemadePoint(userId);

        //    return Ok(response.Data);
        //}
    }
}
