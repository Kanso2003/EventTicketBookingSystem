namespace EventTicketBookingSystem.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }  // Primary Key
        public int TicketId { get; set; }  // Foreign Key to Ticket
        public int UserId { get; set; }  // Foreign Key to User
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }  // e.g., 'credit_card', 'paypal'
        public string PaymentStatus { get; set; }  // e.g., 'successful', 'failed', 'pending'
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow; // Default to now

        // Navigation Properties
        public virtual Ticket Ticket { get; set; }  // Navigation property to Ticket
        public virtual User User { get; set; }  // Navigation property to User
    }
}
