using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class FurnitureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Cost { get; set; }
    }

    public class AddFurnitureToRoomDTO
    {
        public int RoomId { get; set; }
        public List<RoomFurnitureDTO> Furnitures { get; set; } = new List<RoomFurnitureDTO>();
    }

    public class RoomFurnitureDTO
    {
        [Required]
        public int FurnitureId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        public string FurnitureName { get; set; }
    }

    public class AddFurnitureDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Cost { get; set; }

    }
}
