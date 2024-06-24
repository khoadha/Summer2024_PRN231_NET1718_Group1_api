using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.PaymentTransactionRepository;

namespace Hosteland.Services.VnPayService
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentTransactionRepository _transactionRepository;

        public VnPayService(IConfiguration configuration, IPaymentTransactionRepository transactionRepository)
        {
            _configuration = configuration;
            _transactionRepository = transactionRepository;
        }

        public void HandlePaymentSuccess(string transactionId)
        {
            _transactionRepository.HandlePaymentSuccess(transactionId);
        }

        public async Task<ServiceResponse<List<PaymentTransaction>>> GetPaymentTransactions(int? count)
        {
            var serviceResponse = new ServiceResponse<List<PaymentTransaction>>();
            try
            {
                var data = await _transactionRepository.GetPaymentTransactions(count);
                serviceResponse.Data = data;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        public async Task<string> CreatePaymentUrl(PaymentInformationModel model, string userId, HttpContext context, string host, List<Fee> fees)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = host + "pcb";

            PaymentTransaction pt = new()
            {
                VnPayTransactionId = tick,
                UserId = userId,
                Amount = model.Amount,
                Description = model.OrderDescription,
                TransactionStatus = TransactionStatus.Pending, //Pending
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById),
                Fees = fees
            };

            await _transactionRepository.AddPaymentTransaction(pt);

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.OrderDescription}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            return paymentUrl;
        }



        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }

        public async Task<ServiceResponse<List<PaymentTransaction>>> GetTransactionsByUserId(string? userId) {
            var serviceResponse = new ServiceResponse<List<PaymentTransaction>>();
            try {
                var data = await _transactionRepository.GetTransactionsByUserId(userId);
                serviceResponse.Data = data;
            } catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
