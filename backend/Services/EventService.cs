using Backend.Models;
using Backend.Dtos;
using Backend.Data;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> SearchEventsAsync(EventSearchDto searchDto)
        {
            var query = _context.Events.AsQueryable();

            if (searchDto.Date.HasValue)
            {
                query = query.Where(e => e.Date.Date == searchDto.Date.Value.Date);
            }

            if (!string.IsNullOrEmpty(searchDto.Category))
            {
                query = query.Where(e => e.Category == searchDto.Category);
            }

            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(e => e.Price >= searchDto.MinPrice.Value);
            }

            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(e => e.Price <= searchDto.MaxPrice.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> IsEventBookedByUserAsync(Guid eventId, string userId)
        {
            return await _context.Bookings.AnyAsync(b => b.EventId == eventId && b.UserId == userId);
        }

        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _context.Events.FindAsync(id);
        }
    }
}