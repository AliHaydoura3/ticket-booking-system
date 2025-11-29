using Backend.Models;
using Backend.Dtos;

namespace Backend.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> SearchEventsAsync(EventSearchDto searchDto); 
        Task<Event?> GetEventByIdAsync(Guid id);
    }
}