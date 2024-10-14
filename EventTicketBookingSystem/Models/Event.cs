namespace EventTicketBookingSystem.Models
{
    public class Event
    {
        public int EventId { get; set; }  // Primary Key
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Default value
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // Default value

        public ICollection<Ticket> Tickets { get; set; }
    }
}
