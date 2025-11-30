namespace Backend.Dtos
{
    public class BookingDto
    {
        public Guid BookingId { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
    }
}