using System.ComponentModel;

namespace Backend.Models
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}