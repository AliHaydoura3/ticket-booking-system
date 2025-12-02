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

        public async Task<Event?> CreateEventAsync(EventCreateDto dto, string organizerId)
        {
            var newEvent = new Event
            {
                OrganizerId = organizerId,
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                Date = dto.Date,
                Price = dto.Price,
                TotalSeats = dto.TotalSeats,
                AvailableSeats = dto.TotalSeats
            };

            _context.Events.Add(newEvent);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? newEvent : null;
        }

        public async Task<bool> UpdateEventAsync(Guid id, EventCreateDto dto)
        {
            var eventToUpdate = await _context.Events.FindAsync(id);
            if (eventToUpdate == null)
            {
                return false;
            }

            eventToUpdate.Name = dto.Name;
            eventToUpdate.Category = dto.Category;
            eventToUpdate.Description = dto.Description;
            eventToUpdate.Date = dto.Date;
            eventToUpdate.Price = dto.Price;

            if (dto.TotalSeats < eventToUpdate.TotalSeats)
            {
                int seatsDiff = eventToUpdate.TotalSeats - dto.TotalSeats;
                eventToUpdate.AvailableSeats = Math.Max(0, eventToUpdate.AvailableSeats - seatsDiff);
            }
            else
            {
                int seatsDiff = dto.TotalSeats - eventToUpdate.TotalSeats;
                eventToUpdate.AvailableSeats += seatsDiff;
            }

            eventToUpdate.TotalSeats = dto.TotalSeats;

            _context.Events.Update(eventToUpdate);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteEventAsync(Guid id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null)
            {
                return false;
            }

            _context.Events.Remove(eventToDelete);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.Events
                .Select(e => e.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByOrganizerAsync(string organizerId)
        {
            return await _context.Events
                .Where(e => e.OrganizerId == organizerId)
                .ToListAsync();
        }
    }
}