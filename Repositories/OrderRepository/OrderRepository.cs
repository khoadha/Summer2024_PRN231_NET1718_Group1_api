using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;

namespace Repositories.OrderRepository {
    public class OrderRepository : IOrderRepository {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<List<Order>> GetOrders() {
            var list = await _context.Order
                .Include(o => o.User)
                .Include(o => o.Room)
                .Include(o => o.Guests)
                .Include(o => o.Fees)
                .Include(o => o.RoomServices)
                .ThenInclude(rs => rs.Service)
                .Include(o => o.Contracts)
                    .ThenInclude(c => c.Type)
                .ToListAsync();
            return list;
        }

        public async Task<Order> GetOrderById(int id) {
            var order = await _context.Order
                .Include(o => o.User)
                .Include(o => o.Room)
                .Include(o => o.Guests)
                .Include(o => o.Fees)
                .Include(o => o.RoomServices)
                .ThenInclude(rs => rs.Service)
                .Include(o => o.Contracts)
                    .ThenInclude(c => c.Type)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public async Task<List<Order>> GetOrdersByRoomId(int roomId) {
            var list = await _context.Order.Where(o => o.RoomId == roomId)
                .Include(o => o.Contracts)
                .Include(o => o.Guests).ToListAsync();
            return list;
        }

        public async Task<Order> AddOrder(Order order) {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> CreateOrder(Order order, List<Contract> contract, List<Fee> fee) {
            order.Status = OrderStatus.Processing;
            order.RefundStatus = RefundStatus.None;
            order.OrderDate = DateTime.Now;
            var orderId = order.Id;

            for (var i = 0; i < contract.Count; i++) {
                contract[i].OrderId = orderId;
                try {
                    order.Contracts.Add(contract[i]);
                } catch {
                    throw new Exception("Add contract failed");
                }
            }

            for (var i = 0; i < fee.Count; i++) {
                fee[i].OrderId = orderId;
                try {
                    order.Fees.Add(fee[i]);
                } catch {
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

        public async Task<Order> EditOrder(Order order) {
            var existingOrder = await _context.Order.FindAsync(order.Id);
            if (existingOrder != null) {
                existingOrder.CancelDate = order.CancelDate;
                existingOrder.Status = order.Status;

                await _context.SaveChangesAsync();
            }
            return existingOrder;
        }
        public async Task<bool> DeleteOrder(int orderId) {
            var order = await _context.Order.FindAsync(orderId);
            if (order != null) {
                _context.Order.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Order> FindOrderById(int id) {
            return await _context.Order
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ContractType>> GetContractTypes() {
            return await _context.ContractTypes.ToListAsync();
        }

        public async Task<ContractType> AddContractType(ContractType order) {
            try {
                await _context.ContractTypes.AddAsync(order);
                await SaveAsync();
            } catch (Exception ex) {
                throw;
            }
            return order;
        }

        public async Task<List<FeeCategory>> GetFeeCates() {
            return await _context.FeeCategories.ToListAsync();
        }

        public async Task<FeeCategory> AddFeeCate(FeeCategory order) {
            try {
                await _context.FeeCategories.AddAsync(order);
                await SaveAsync();
            } catch (Exception ex) {
                throw;
            }
            return order;
        }

        public async Task<List<Fee>> GetFees() {
            return await _context.Fees
                .Include(f => f.FeeCategory)
                .ToListAsync();
        }

        public async Task<List<Fee>> GetFeesByOrderId(int orderId) {
            return await _context.Fees
                .Include(f => f.FeeCategory)
                .Where(f => f.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<bool> SaveAsync() {
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<Fee> GetFeeById(int id) {
            return await _context.Fees.Include(a => a.FeeCategory).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task TriggerMonthlyBill(CancellationToken cancellationToken) {
            using (var transaction = await _context.Database.BeginTransactionAsync()) {
                try {

                    int ROOM_FEE_CATEGORY_ID = 1;
                    int ELECTRIC_FEE_CATEGORY_ID = 6;

                    var feeCates = await _context.FeeCategories.ToListAsync(cancellationToken);

                    var monthlyBillOrders = await _context.Order
                    .Include(o => o.Room)
                    .Where(a => a.IsMonthly)
                    .ToListAsync(cancellationToken);
                    var totalDaysToBill = CalculateTotalDays();

                    var now = DateTime.Now;

                    foreach (var order in monthlyBillOrders) {
                        if (order.Fees is not null) {
                            Fee roomFee = new();
                            roomFee.FeeCategoryId = feeCates.Find(c => c.Id == ROOM_FEE_CATEGORY_ID).Id;
                            roomFee.FeeCategory = feeCates.Find(c => c.Id == ROOM_FEE_CATEGORY_ID);
                            roomFee.FeeStatus = FeeStatus.Unpaid;
                            roomFee.PaymentDate = now.AddDays(7);
                            roomFee.Amount = order.Room.CostPerDay * totalDaysToBill * 0.9; //Thue theo thang *0.9 so voi thue theo ngay
                            roomFee.Name = $"Room Fee {now.Month}/{now.Year}";
                            order.Fees.Add(roomFee);

                            Fee electricFee = new();
                            electricFee.FeeCategoryId = feeCates.Find(c => c.Id == ELECTRIC_FEE_CATEGORY_ID).Id;
                            electricFee.FeeCategory = feeCates.Find(c => c.Id == ELECTRIC_FEE_CATEGORY_ID);
                            electricFee.FeeStatus = FeeStatus.Deferred;
                            electricFee.PaymentDate = now.AddDays(10);
                            electricFee.Amount = 0;
                            electricFee.Name = $"Electric Fee {now.Month}/{now.Year}";
                            order.Fees.Add(electricFee);
                        }
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    await transaction.CommitAsync();
                } catch (OperationCanceledException) {
                    await transaction.RollbackAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    throw;
                } finally {
                    await transaction.DisposeAsync();
                }
            }
        }

        public async Task<List<Fee>> GetDeferredElectricityFee() {
            int ELECTRIC_FEE_CATEGORY_ID = 6;
            return await _context.Fees
                .Include(a => a.Order)
                .ThenInclude(a => a.Room)
                .Where(a => a.FeeCategoryId == ELECTRIC_FEE_CATEGORY_ID &&
                       a.FeeStatus == FeeStatus.Deferred)
                .ToListAsync();
        }

        public async Task UpdateAmountFee(UpdateAmountFeeRequestDTO dto) {
            if (dto.Children is not null) {
                var feeIds = dto.Children.Select(c => c.Id).ToList();

                var fees = await _context.Fees
            .Where(a => feeIds.Contains(a.Id))
            .ToListAsync();

                foreach (var fee in fees) {
                    var matchingChild = dto.Children.Find(c => c.Id == fee.Id);
                    if (matchingChild != null) {
                        fee.Amount = matchingChild.Amount;
                        fee.FeeStatus = FeeStatus.Unpaid;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        //Calculate total days between 15 this month and 15 next month (tao bill ngay 15 hang thang)
        private static int CalculateTotalDays() {
            DateTime currentDate = DateTime.Now;
            DateTime startOfCurrentMonth = new(currentDate.Year, currentDate.Month, 15);
            DateTime startOfNextMonth;
            if (currentDate.Month == 12) {
                startOfNextMonth = new DateTime(currentDate.Year + 1, 1, 15);
            } else {
                startOfNextMonth = new DateTime(currentDate.Year, currentDate.Month + 1, 15);
            }
            int totalDays = (startOfNextMonth - startOfCurrentMonth).Days;
            return totalDays;
        }
    }
}
