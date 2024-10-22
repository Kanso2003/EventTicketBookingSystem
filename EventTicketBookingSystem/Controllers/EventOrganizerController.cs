using EventTicketBookingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EventTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventOrganizerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventOrganizerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create organizer profile
        [HttpPost]
        public async Task<IActionResult> CreateOrganizer([FromBody] EventOrganizer organizer)
        {
            if (organizer == null)
            {
                return BadRequest(new { message = "Organizer data is required." });
            }

            _context.EventOrganizers.Add(organizer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrganizerById), new { id = organizer.OrganizerId }, organizer);
        }

        // Read organizer details
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizerById(int id)
        {
            var organizer = await _context.EventOrganizers.FindAsync(id);

            if (organizer == null)
            {
                return NotFound(new { message = "Organizer not found." });
            }

            return Ok(organizer);
        }

        // Update organizer information
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganizer(int id, [FromBody] EventOrganizer organizer)
        {
            if (id != organizer.OrganizerId)
            {
                return BadRequest(new { message = "Organizer ID mismatch." });
            }

            var existingOrganizer = await _context.EventOrganizers.FindAsync(id);
            if (existingOrganizer == null)
            {
                return NotFound(new { message = "Organizer not found." });
            }

            existingOrganizer.OrganizerName = organizer.OrganizerName;
            existingOrganizer.ContactEmail = organizer.ContactEmail;
            existingOrganizer.PhoneNumber = organizer.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Organizer updated successfully.", organizer = existingOrganizer });
        }

        // Delete organizer profile
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizer(int id)
        {
            var organizer = await _context.EventOrganizers.FindAsync(id);
            if (organizer == null)
            {
                return NotFound(new { message = "Organizer not found." });
            }

            _context.EventOrganizers.Remove(organizer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
