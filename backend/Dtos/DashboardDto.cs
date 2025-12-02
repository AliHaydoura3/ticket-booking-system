namespace Backend.Dtos
{
    public class DashboardDto
    {
        public int TotalEvents { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalUsers { get; set; }
    }
}