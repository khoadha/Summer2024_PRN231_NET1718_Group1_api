using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public double Cost { get; set; }
        public List<GuestDto> Guests { get; set; }
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
