using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Services.Interfaces;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> CreateBooking(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
    
            var result = await _bookingService.CreateBookingAsync(id, userId);
            if (!result)
            {
                return BadRequest("Failed to create booking.");
            }

            return Ok("Booking created successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _bookingService.CancelBookingAsync(id, userId);
            if (!result)
            {
                return BadRequest("Failed to cancel booking.");
            }

            return Ok("Booking canceled successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBookings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }
    }
}