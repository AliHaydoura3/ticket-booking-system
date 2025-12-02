using Microsoft.AspNetCore.Mvc;
using Backend.Services.Interfaces;
using Backend.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService = null!;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto dto)
        {
            var organizerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(organizerId))
            {
                return Unauthorized();
            }

            var createdEvent = await _eventService.CreateEventAsync(dto, organizerId);
            if (createdEvent == null)
            {
                return BadRequest("Failed to create event.");
            }

            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.EventId }, createdEvent);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventCreateDto dto)
        {
            var success = await _eventService.UpdateEventAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _eventService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var success = await _eventService.DeleteEventAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            var isBooked = await _eventService.IsEventBookedByUserAsync(id, userId);
            return Ok(new { eventItem, isBooked });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetEventsByOrganizer()
        {
            var organizerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(organizerId))
            {
                return Unauthorized();
            }

            var events = await _eventService.GetEventsByOrganizerAsync(organizerId);
            return Ok(events);
        }
    }
}
