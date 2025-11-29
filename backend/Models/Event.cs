namespace Backend.Models
{
    public class Event
    {
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}