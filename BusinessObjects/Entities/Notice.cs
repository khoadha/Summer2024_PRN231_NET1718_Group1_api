using BusinessObjects.ConfigurationModels;
namespace BusinessObjects.Entities {
    public class Notice : BaseEntity {
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsReaded { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
