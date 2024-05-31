using BusinessObjects.ConfigurationModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.Entities {
    public class Room : BaseEntity {
        public string? Name { get; set; }
        public int RoomSize { get; set; }
        public string? RoomDescription { get; set; }
        public double RoomArea { get; set; }
        public double CostPerDay { get; set; }
        public string? Location { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
        public virtual RoomCategory? Category { get; set; }
        public virtual ICollection<RoomFurniture>? RoomFurniture { get; set; }
        public virtual ICollection<RoomImage>? RoomImages { get; set; }
    }


    public class RoomCategory : BaseEntity { 
        public string? CategoryName { get; set; }
    }

    public class RoomFurniture : BaseEntity {
        public int FurnitureId { get; set; }
        public int RoomId { get; set; }
        public int Quantity { get; set; }
        public virtual Room? Room { get; set; }
        public virtual Furniture? Furniture { get; set; }
    }

    public class Furniture : BaseEntity {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Cost { get; set; }
        public virtual ICollection<RoomFurniture>? RoomFurniture { get; set; }
    }

    public class RoomImage : BaseEntity {
        public string? Url { get; set; }
    }
}
