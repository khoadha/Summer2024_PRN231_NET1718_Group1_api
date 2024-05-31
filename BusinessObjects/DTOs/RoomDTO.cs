using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GetRoomDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
        public int RoomSize { get; set; }
        public double RoomArea { get; set; }
        public double CostPerDay { get; set; }
        public string? Location { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }

    public class FurnitureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class RoomFurnitureDTO
    {
        public int FurnitureId { get; set; }
        public int RoomId { get; set; }
        public FurnitureDTO Furniture { get; set; }
        public int Quantity { get; set; } 
    }

    public class ImageDTO
    {
        public int Id { get; set; }
        public string ImgPath { get; set; } = string.Empty;
    }


    public class GetRoomDetailDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int RoomSize { get; set; }
        public double RoomArea { get; set; }
        public string RoomDescription { get; set; } = string.Empty;
        public double CostPerDay { get; set; }
        public string? Location { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public List<RoomFurnitureDTO> RoomFurniture { get; set; } = new List<RoomFurnitureDTO>();
        public List<ImageDTO> RoomImages { get; set; } = new List<ImageDTO>();
    }
}
