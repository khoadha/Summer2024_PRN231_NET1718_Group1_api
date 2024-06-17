using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PaymentTransactionRepository
{
    public interface IPaymentTransactionRepository
    {
        void HandlePaymentSuccess(string txnRef);
        Task<List<PaymentTransaction>> GetPaymentTransactions(int? count);
        Task<PaymentTransaction> AddPaymentTransaction(PaymentTransaction pt);
        Task<bool> SaveAsync();
    }
}
