namespace EventTicketBookingSystem.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }  // Primary Key
        public int PaymentId { get; set; }  // Foreign Key
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;  // Default value
        public decimal TransactionAmount { get; set; }

        public Payment Payment { get; set; }
    }
}
