using BusinessObjects.ConfigurationModels;
namespace BusinessObjects.Entities {
    public class BackgroundTask : BaseEntity {
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ExecutedDay { get; set; }
        public int ExecutedMonth { get; set; }
        public int ExecutedYear { get; set; }
        public bool IsSuccess { get; set; }
    }
}
