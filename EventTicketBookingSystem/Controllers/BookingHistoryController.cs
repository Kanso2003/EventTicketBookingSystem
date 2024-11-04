using EventTicketBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EventTicketBookingSystem.Models;

namespace EventTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/BookingHistory (Create Booking Entry)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingHistory bookingHistory)
        {
            if (ModelState.IsValid)
            {
                _context.BookingHistories.Add(bookingHistory);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Booking created successfully.", bookingId = bookingHistory.BookingId });
            }
            return BadRequest(ModelState);
        }

        // GET: api/BookingHistory/user/{userId} (Read User's Booking History)
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookingHistory(int userId)
        {
            var userBookings = await _context.BookingHistories
                                             .Where(b => b.UserId == userId)
                                             .ToListAsync();

            if (!userBookings.Any())
                return NotFound(new { message = "No booking history found for this user." });

            return Ok(userBookings);
        }

        // PUT: api/BookingHistory/{id} (Update Booking Status)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] string status)
        {
            var booking = await _context.BookingHistories.FindAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            if (string.IsNullOrEmpty(status))
                return BadRequest(new { message = "Status cannot be empty." });

            booking.Status = status;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Booking status updated successfully.",
                newStatus = booking.Status
            });
        }

        // DELETE: api/BookingHistory/{id} (Delete Booking)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.BookingHistories.FindAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found." });

            _context.BookingHistories.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking deleted successfully." });
        }
    }
}
