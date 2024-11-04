using EventTicketBookingSystem.Models;

public class Event
{
    public int EventId { get; set; } // Primary Key
    public string EventName { get; set; }
    public string EventDescription { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key to EventOrganizer
    public int OrganizerId { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}
