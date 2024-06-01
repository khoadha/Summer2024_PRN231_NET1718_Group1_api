namespace HostelandAuthorization.Shared
{
    public class CreateOrderDto
    {
        public string? UserId { get; set; }
        public double Cost { get; set; }
        public List<GuestDto> Guests { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class GuestDto
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
