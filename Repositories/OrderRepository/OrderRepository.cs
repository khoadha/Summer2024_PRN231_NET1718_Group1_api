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
                .Include(o => o.Guests).ToListAsync();
            return list;
        }

        public async Task<Order> AddOrder(Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> CreateOrder(Order order, Contract contract)
        {
            order.Status = OrderStatus.Processing;
            order.OrderDate = DateTime.Now;
            order.RoomId = 2;
            contract.OrderId = order.Id;
            order.Contracts.Add(contract);
            _context.Order.Add(order);
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
    }
}
