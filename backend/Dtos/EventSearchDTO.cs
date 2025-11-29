namespace Backend.Dtos
{
    public class EventSearchDto
    {
        public DateTime? Date { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}