using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Backend.Dtos;

namespace Backend.Services
{
    [Authorize]
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBookingAsync(Guid eventId, string userId)
        {
            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity == null || eventEntity.AvailableSeats <= 0)
            {
                return false;
            }

            bool exists = await _context.Bookings
                .AnyAsync(b => b.EventId == eventId && b.UserId == userId);

            if (exists)
                return false;

            eventEntity.AvailableSeats = Math.Max(0, eventEntity.AvailableSeats - 1);

            _context.Bookings.Add(new Booking
            {
                EventId = eventId,
                UserId = userId,
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelBookingAsync(Guid eventId, string userId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.EventId == eventId && b.UserId == userId);

            if (booking == null)
            {
                return false;
            }

            var eventEntity = await _context.Events.FindAsync(booking.EventId);
            if (eventEntity != null)
            {
                eventEntity.AvailableSeats += 1;
            }

            _context.Bookings.Remove(booking);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Select(b => new BookingDto
                {
                    BookingId = b.BookingId,
                    EventId = b.EventId,
                    EventName = b.Event.Name,
                    EventDate = b.Event.Date,
                })
                .ToListAsync();
        }

        public async Task<DashboardDto> GetDashboardDataAsync(string organizerId)
        {
            var totalEvents = await _context.Events
                .CountAsync(e => e.OrganizerId == organizerId);

            var totalBookings = await _context.Bookings
                .Where(b => b.Event.OrganizerId == organizerId)
                .CountAsync();

            var totalRevenue = await _context.Bookings
                .Where(b => b.Event.OrganizerId == organizerId)
                .SumAsync(b => b.Event.Price);

            var totalUsers = await _context.Users.CountAsync(u => 
                _context.Bookings.Any(b => b.UserId == u.Id && b.Event.OrganizerId == organizerId));

            return new DashboardDto
            {
                TotalEvents = totalEvents,
                TotalBookings = totalBookings,
                TotalRevenue = totalRevenue,
                TotalUsers = totalUsers
            };
        }
    }
}