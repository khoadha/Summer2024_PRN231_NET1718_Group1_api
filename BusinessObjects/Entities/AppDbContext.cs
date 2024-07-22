using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects.Entities {
    public class AppDbContext : IdentityDbContext<ApplicationUser> {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Fee> Fees { get; set; }
        public virtual DbSet<FeeCategory> FeeCategories { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomService> RoomServices { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServicePrice> ServicePrices { get; set; }
        public virtual DbSet<RoomCategory> RoomCategories { get; set; }
        public virtual DbSet<RoomFurniture> RoomFurniture { get; set; }
        public virtual DbSet<Furniture> Furniture { get; set; }
        public virtual DbSet<RoomImage> RoomImages { get; set; }
        public virtual DbSet<GlobalRate> GlobalRates { get; set; }
        public virtual DbSet<ContractType> ContractTypes { get; set; }
        public virtual DbSet<BackgroundTask> BackgroundTasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }

        private static string GetConnectionString() {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json", true, true)
                 .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];
            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }

    }
}
