using BusinessObjects.Entities;

namespace Repositories.PaymentTransactionRepository
{
    public interface IPaymentTransactionRepository
    {
        void HandlePaymentSuccess(string txnRef);
        Task<List<PaymentTransaction>> GetPaymentTransactions(int? count);
        Task<List<PaymentTransaction>> GetTransactionsByUserId(string? userId);
        Task<PaymentTransaction> AddPaymentTransaction(PaymentTransaction pt);
        Task<bool> SaveAsync();

        Task<int> GetTotalTransactionCount();
    }
}
