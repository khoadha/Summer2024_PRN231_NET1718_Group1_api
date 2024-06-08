using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<RoomFurnitureDTO> RoomFurniture { get; set; } = new List<RoomFurnitureDTO>();

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

    public class AddRoomDTO
    {
        [Required]
        public string? Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Room size must be greater than 0")]
        public int RoomSize { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Room area must be greater than 0")]
        public double RoomArea { get; set; }

        [Required]
        public string RoomDescription { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Cost per day must be non-negative")]
        public double CostPerDay { get; set; }

        public string? Location { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }

    public class UpdateRoomDTO
    {
        public int RoomId { get; set; }
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Room size must be greater than 0")]
        public int RoomSize { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Room area must be greater than 0")]
        public double RoomArea { get; set; }

        public string RoomDescription { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cost per day must be non-negative")]
        public double CostPerDay { get; set; }
        public string? Location { get; set; }

        public int CategoryId { get; set; }
    }

}
