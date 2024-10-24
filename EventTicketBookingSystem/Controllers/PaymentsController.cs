using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventTicketBookingSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout; // Add Stripe Checkout for session creation

namespace EventTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentsController(IConfiguration configuration, ApplicationDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        // Create Payment with Stripe
        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] Payment payment)
        {
            // Validate TicketId and UserId
            if (!await _context.Tickets.AnyAsync(t => t.TicketId == payment.TicketId))
            {
                return BadRequest("Invalid Ticket ID");
            }

            if (!await _context.Users.AnyAsync(u => u.UserId == payment.UserId))
            {
                return BadRequest("Invalid User ID");
            }

            // Stripe payment processing
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(payment.Amount * 100), // Stripe uses cents, so multiply by 100
                Currency = "usd", // You can change the currency as needed
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", payment.UserId.ToString() },
                    { "TicketId", payment.TicketId.ToString() }
                }
            };

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            try
            {
                paymentIntent = service.Create(options);
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.Message });
            }

            // Save payment info to the database after successful payment intent creation
            payment.PaymentIntentId = paymentIntent.Id; // Store Stripe's payment intent ID
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, payment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Ticket) // Include Ticket
                .Include(p => p.User)   // Include User
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments
                .Include(p => p.Ticket) // Include Ticket
                .Include(p => p.User)   // Include User
                .ToListAsync();
        }

        // Update Payment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Delete Payment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
