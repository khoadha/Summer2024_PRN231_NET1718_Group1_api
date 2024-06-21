using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrders()
        {
            var list = await _context.Order
                .Include(o => o.Contracts)
                .Include(o => o.User)
                .Include(o => o.Room)
                .Include(o => o.Guests)
                .Include(o =>o.Fees)
                .ToListAsync();
            return list;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _context.Order
                .Include(o => o.Contracts)
                .Include(o => o.Guests)
                .Include(o => o.User)
                .Include(o => o.Room)
                .Include(o => o.Fees)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public async Task<List<Order>> GetOrdersByRoomId(int roomId)
        {
            var list = await _context.Order.Where(o => o.RoomId == roomId)
                .Include(o => o.Contracts)
                .Include(o => o.Guests).ToListAsync();
            return list;
        }

        public async Task<Order> AddOrder(Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> CreateOrder(Order order, List<Contract> contract, List<Fee> fee)
        {
            order.Status = OrderStatus.Processing;
            order.RefundStatus = RefundStatus.None;
            order.OrderDate = DateTime.Now;
            var orderId = order.Id;

            for(var i = 0; i < contract.Count; i++)
            {
                contract[i].OrderId = orderId;
                try
                { 
                    order.Contracts.Add(contract[i]);
                }
                catch
                {
                    throw new Exception("Add contract failed");
                }
            }
            
            for(var i = 0; i < fee.Count; i++)
            {
                fee[i].OrderId = orderId;
                try
                { 
                    order.Fees.Add(fee[i]);
                }
                catch
                {
                    throw new Exception("Add fee failed");
                }
            }
            _context.Order.Add(order);
            /*
            //Update ROom Status
            var _room = _context.Rooms.FirstOrDefault(r => r.Id.Equals(order.Id));
            _room.IsAvailable = false;
            */
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> EditOrder(Order order)
        {
            var existingOrder = await _context.Order.FindAsync(order.Id);
            if (existingOrder != null)
            {
                existingOrder.CancelDate = order.CancelDate;
                existingOrder.Status = order.Status;

                await _context.SaveChangesAsync();
            }
            return existingOrder;
        }
        public async Task<bool> DeleteOrder(int orderId)
        {
            var order = await _context.Order.FindAsync(orderId);
            if (order != null)
            {
                _context.Order.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Order> FindOrderById(int id)
        {
            return await _context.Order
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ContractType>> GetContractTypes()
        {
            return await _context.ContractTypes.ToListAsync();
        }

        public async Task<ContractType> AddContractType(ContractType order)
        {
            try
            {
                await _context.ContractTypes.AddAsync(order);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return order;
        }
        
        public async Task<List<FeeCategory>> GetFeeCates()
        {
            return await _context.FeeCategories.ToListAsync();
        }

        public async Task<FeeCategory> AddFeeCate(FeeCategory order)
        {
            try
            {
                await _context.FeeCategories.AddAsync(order);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return order;
        }

        public async Task<List<Fee>> GetFees()
        {
            return await _context.Fees
                .Include(f => f.FeeCategory)
                .ToListAsync();
        }
        
        public async Task<List<Fee>> GetFeesByOrderId(int orderId)
        {
            return await _context.Fees
                .Include(f=> f.FeeCategory)
                .Where(f => f.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;

        }
    }
}
