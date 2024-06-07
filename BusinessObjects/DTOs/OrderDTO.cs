using BusinessObjects.Entities;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public double Cost { get; set; }
        public List<GuestDto> Guests { get; set; }
        public List<Service> RoomServices { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
    }
    public class GuestDto
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
