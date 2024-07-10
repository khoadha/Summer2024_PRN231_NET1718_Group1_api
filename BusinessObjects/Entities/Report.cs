using BusinessObjects.ConfigurationModels;
using BusinessObjects.Enums;

namespace BusinessObjects.Entities {
    public class Report : BaseEntity {
        public int RoomId { get; set; }
        public string? AuthorId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reply { get; set; }
        public virtual ApplicationUser? Author { get; set; }
        public virtual Room? Room { get; set; }
        public MaintainanceStatus Status { get; set; }
    }
}
