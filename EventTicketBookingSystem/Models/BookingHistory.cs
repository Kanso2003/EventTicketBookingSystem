using System.ComponentModel.DataAnnotations;

namespace EventTicketBookingSystem.Models
{
    public class BookingHistory
    {
        [Key]
        public int BookingId { get; set; } // Primary Key

        public int UserId { get; set; } // Foreign Key
        public int TicketId { get; set; } // Foreign Key
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } // Status ENUM can be defined as a string

        // Navigation properties
        public User User { get; set; }
        public Ticket Ticket { get; set; }
    }
}

