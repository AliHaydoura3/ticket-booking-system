using Backend.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CreateBookingAsync(Guid eventId, string userId);
        Task<bool> CancelBookingAsync(Guid bookingId, string userId);
        Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId);
    }
}