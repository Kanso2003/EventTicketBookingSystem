using EventTicketBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EventTicketBookingSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        [Authorize]  
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET: api/events/1
        [Authorize] 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var singleEvent = await _context.Events.FindAsync(id);
            if (singleEvent == null) return NotFound();
            return Ok(singleEvent);
        }

        // POST: api/events
        [Authorize(Roles = "Admin")]  // Only "Admin" role can access this
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.EventId }, newEvent);
        }

        // PUT: api/events/1
        [Authorize(Roles = "Admin")]  // Only "Admin" role can access this
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            var eventToUpdate = await _context.Events.FindAsync(id);
            if (eventToUpdate == null) return NotFound();

            // Update the properties
            eventToUpdate.EventName = updatedEvent.EventName;
            eventToUpdate.EventDescription = updatedEvent.EventDescription;
            eventToUpdate.EventDate = updatedEvent.EventDate;
            eventToUpdate.Location = updatedEvent.Location;
            eventToUpdate.UpdatedAt = DateTime.UtcNow; // Set updated timestamp

            await _context.SaveChangesAsync();
            return Ok(eventToUpdate);
        }

        // DELETE: api/events/1
        [Authorize(Roles = "Admin")]  // Only "Admin" role can access this
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null) return NotFound();

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return NoContent(); // Return 204 No Content
        }
    }
}
