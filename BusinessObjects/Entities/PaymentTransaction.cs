using BusinessObjects.ConfigurationModels;
using BusinessObjects.Enums;
namespace BusinessObjects.Entities {
    public class PaymentTransaction : BaseEntity {
        public string? VnPayTransactionId { get; set; }
        public string? UserId { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public TransactionStatus? TransactionStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ApplicationUser? User { get; set; }
        public virtual ICollection<Fee>? Fees { get; set; } = new List<Fee>();
    }
}
