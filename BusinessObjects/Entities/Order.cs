using BusinessObjects.ConfigurationModels;
using BusinessObjects.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities {
    public class Order : BaseEntity {
        public string? UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public OrderStatus Status { get; set; }
        public RefundStatus RefundStatus {  get; set; }
        public virtual Room? Room { get; set; }
        public virtual ICollection<Contract>? Contracts { get; set; } = new List<Contract>();
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<RoomService>? RoomServices { get; set; }
        public virtual ICollection<Fee>? Fees { get; set; } = new List<Fee>();
        public virtual ICollection<Guest>? Guests { get; set; }
    }

    public class RoomService : BaseEntity {
        public int OrderId { get; set; }
        public int ServiceId { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Service? Service { get; set; }
    }

    public class Service : BaseEntity {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<RoomService>? RoomServices { get; set; }
        public virtual ICollection<ServicePrice>? ServicePrice { get; set; }
    }

    public class ServicePrice : BaseEntity {
        public int ServiceId { get; set; }
        public double Amount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual Service Service { get; set; }
    }

    public class Contract : BaseEntity {
        public string? Name { get; set; }
        public int OrderId { get; set; }
        public double Cost { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Order Order { get; set; }

        [ForeignKey("ContractTypeId")]
        public int ContractTypeId { get; set; }
        public virtual ContractType? Type { get; set; }

    }

    public class ContractType : BaseEntity
    {
        public string? ContractName { get; set; }
    }

    public class Guest : BaseEntity {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Relationship { get; set; } //With people who order
        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }

    public class Fee : BaseEntity {
        public string? Name {  get; set; }
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public int FeeCategoryId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public FeeStatus FeeStatus { get; set; }
        public virtual Order? Order { get; set; }
        public virtual FeeCategory? FeeCategory { get; set; }
        [ForeignKey("PaymentTransactionId")]
        public int? PaymentTransactionId { get; set; }
        public virtual PaymentTransaction? PaymentTransaction { get; set; }
    }

    public class FeeCategory : BaseEntity {
        public string? Name { get; set; }
    }

    public class GlobalRate : BaseEntity
    {
        public float? Deposit { get; set; }
        public float? Refund { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
