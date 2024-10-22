namespace EventTicketBookingSystem.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }  // Primary Key
        public int EventId { get; set; }  // Foreign Key to Event
        public int? UserId { get; set; }  // Nullable Foreign Key to User
        public string TicketType { get; set; }  // e.g., 'regular', 'vip'
        public decimal Price { get; set; }
        public string Status { get; set; }  // e.g., 'available', 'sold', 'reserved'
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to now
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Default to now

        // Navigation Properties
        public virtual Event Event { get; set; }  // Navigation property to Event
        public virtual User User { get; set; }  // Navigation property to User
        public virtual Payment Payment { get; set; }  // Navigation property to Payment
        public virtual ICollection<BookingHistory> BookingHistories { get; set; }  // Navigation property to BookingHistories
        public virtual ICollection<Payment> Payments { get; set; }  // Navigation property
    }
}
