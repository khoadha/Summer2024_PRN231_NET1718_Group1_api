using BusinessObjects.ConfigurationModels;
using BusinessObjects.Entities;

namespace Hosteland.Services.VnPayService
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(PaymentInformationModel model, string userId, HttpContext context, string host);
        Task<ServiceResponse<List<PaymentTransaction>>> GetPaymentTransactions(int? count);
        void HandlePaymentSuccess(string transactionId);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
