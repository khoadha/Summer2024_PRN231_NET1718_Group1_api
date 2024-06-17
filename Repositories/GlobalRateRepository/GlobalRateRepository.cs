using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.GlobalRateRepository
{
    public class GlobalRateRepository : IGlobalRateRepository
    {
        private readonly AppDbContext _context;

        public GlobalRateRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<GlobalRate> GetRateById(int id)
        {
            GlobalRate result = new GlobalRate();
            try
            {
                result = await _context.GlobalRates.FirstOrDefaultAsync(c => c.Id.Equals(id));
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public async Task<GlobalRate> GetNewestRateAsync()
        {
            return await _context.GlobalRates
                .OrderByDescending(rate => rate.StartDate)
                .FirstOrDefaultAsync();
        }
        public async Task<List<GlobalRate>> GetRates()
        {
            List<GlobalRate> result = new List<GlobalRate>();
            try
            {
                result = await _context.GlobalRates.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<GlobalRate> AddRate(GlobalRate rate)
        {
            try
            {
                // Update the end date of the latest rate
                var latestRate = await _context.GlobalRates
                   .OrderByDescending(sp => sp.StartDate)
                   .FirstOrDefaultAsync();

                if (latestRate != null && latestRate.EndDate == null)
                {
                    latestRate.EndDate = DateTime.Now;
                    _context.GlobalRates.Update(latestRate);
                }

                await _context.GlobalRates.AddAsync(rate);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return rate;
        }
    }
}
