using Microsoft.AspNetCore.Mvc;
using Backend.Services.Interfaces;
using Backend.Dtos;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService = null!;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchEvents([FromQuery] EventSearchDto searchDto)
        {
            var results = await _eventService.SearchEventsAsync(searchDto);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return Ok(eventItem);
        }
    }
}
