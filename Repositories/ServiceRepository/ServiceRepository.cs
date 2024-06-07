using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ServiceRepository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Service> AddService(Service service)
        {
            try
            {
                await _context.Services.AddAsync(service);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return service;
        }

        public async Task<ServicePrice> CreateServicePrice(ServicePrice servicePrice)
        {
            try
            {
                await _context.ServicePrices.AddAsync(servicePrice);
                await SaveAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
            return servicePrice;

        }

        public async Task<List<Service>> GetServices()
        {
            List<Service> result = new List<Service>();
            try
            {
                result = await _context.Services.Include(s => s.ServicePrice).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        
        public async Task<List<ServicePrice>> GetServicePricesByServiceId(int id)
        {
            List<ServicePrice> result = new List<ServicePrice>();
            try
            {
                result = await _context.ServicePrices
                    .Where(sp => sp.ServiceId.Equals(id))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public async Task<Service> GetServiceById(int id)
        {
            Service result = new Service();
            try
            {
                result = await _context.Services.FirstOrDefaultAsync(c => c.Id == id);
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
    }
}
