using EventTicketBookingSystem.Models;
using System.Text.Json.Serialization;

public class Payment
{
    public int PaymentId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public string PaymentIntentId { get; set; }
    public string StripePaymentId { get; set; }

    [JsonIgnore]  // Ignore these properties during JSON deserialization
    public virtual Ticket? Ticket { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
