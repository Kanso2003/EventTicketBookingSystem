using System.Net.Sockets;

namespace EventTicketBookingSystem.Models
{
    public class User
    {
        public int UserId { get; set; }  // Primary Key
        public string Username { get; set; }  // Unique
        public string Email { get; set; }  // Unique
        public string PasswordHash { get; set; }  // Encrypted Password
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to now
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Default to now

        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<BookingHistory> BookingHistories { get; set; }
    }
}
