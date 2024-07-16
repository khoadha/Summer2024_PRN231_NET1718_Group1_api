using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.PaymentTransactionRepository
{
    public class PaymentTransactionRepository : IPaymentTransactionRepository
    {
        private readonly AppDbContext _context;

        public PaymentTransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactions(int? count)
        {
            if (count.HasValue)
            {
                return await _context.PaymentTransactions.OrderByDescending(a => a.Id).Take(count.Value).ToListAsync();
            }
            return await _context.PaymentTransactions.ToListAsync();
        }

        public async Task<List<PaymentTransaction>> GetPaymentTransactionsByDate(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.PaymentTransactions.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.CreatedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.CreatedDate <= toDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<PaymentTransaction>> GetTopLatestTransactions()
        {
            return await _context.PaymentTransactions
                                 .OrderByDescending(a => a.CreatedDate)
                                 .Take(10)
                                 .ToListAsync();
        }


        public async Task<PaymentTransaction> AddPaymentTransaction(PaymentTransaction transaction)
        {
            try
            {
                var isExistTransaction = await _context.PaymentTransactions.AnyAsync(fc => fc.VnPayTransactionId == transaction.VnPayTransactionId);
                if (!isExistTransaction)
                {
                    await _context.PaymentTransactions.AddAsync(transaction);
                    await SaveAsync();
                }
                else
                {
                    throw new Exception("This transaction is existed.");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return transaction;
        }

        public void HandlePaymentSuccess(string txnRef)
        {
            using var dbTransaction = _context.Database.BeginTransaction();
            try
            {
                PaymentTransaction transaction = _context.PaymentTransactions.First(fc => fc.VnPayTransactionId == txnRef);
                if (transaction != null)
                {
                    var user = _context.Users.FirstOrDefault(a => a.Id == transaction.UserId);
                    if (user is not null)
                    {
                        if (transaction.TransactionStatus == TransactionStatus.Pending)
                        {

                            //user.AccountBalance += ((int)transaction.Amount);
                            //_context.Users.Update(user);


                            transaction.TransactionStatus = TransactionStatus.Success;
                            _context.PaymentTransactions.Update(transaction);
                            _context.SaveChanges();

                            var fees = _context.Fees.Where(f => f.PaymentTransactionId == transaction.Id).ToList();
                            foreach (var fee in fees)
                            {
                                fee.FeeStatus = FeeStatus.Paid;
                                _context.Fees.Update(fee);
                            }
                            _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    throw new Exception("This transaction is not existed.");
                }
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<PaymentTransaction>> GetTransactionsByUserId(string? userId) {
            return await _context.PaymentTransactions.Where(a=>a.UserId==userId).ToListAsync();
        }

        public async Task<int> GetTotalTransactionCount()
        {
            return await _context.PaymentTransactions.CountAsync();
        }
    }
}
