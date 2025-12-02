using Backend.Models;
using Backend.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> SearchEventsAsync(EventSearchDto searchDto); 
        Task<bool> IsEventBookedByUserAsync(Guid eventId, string userId);
        Task<Event?> GetEventByIdAsync(Guid id);
        Task<Event?> CreateEventAsync(EventCreateDto dto, string organizerId);
        Task<bool> UpdateEventAsync(Guid id, EventCreateDto dto);
        Task<bool> DeleteEventAsync(Guid id);
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<IEnumerable<Event>> GetEventsByOrganizerAsync(string organizerId);
    }
}