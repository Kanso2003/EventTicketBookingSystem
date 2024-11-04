using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventTicketBookingSystem.Models;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        // Create Payment and Initialize Stripe Checkout Session
        [HttpPost("checkout-session")]
        public async Task<IActionResult> CreatePayment([FromBody] Payment payment)
        {
            var ticket = await _context.Tickets.FindAsync(payment.TicketId);
            var existingPayment = await _context.Payments
             .Where(p => p.TicketId == payment.TicketId && p.UserId == payment.UserId && p.PaymentStatus == "Pending")
             .FirstOrDefaultAsync();

            if (existingPayment != null)
            {
                return BadRequest("A pending payment already exists for this ticket and user.");
            }

            if (ticket == null)
            {
                return BadRequest("Invalid Ticket ID or Ticket not found");
            }

            var user = await _context.Users.FindAsync(payment.UserId);
            if (user == null)
            {
                return BadRequest("Invalid User ID or User not found");
            }


            // Customize the description with event and ticket details
            var description = $"The Weekend Event - Ticket #{ticket.TicketId}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(ticket.Price * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = ticket.TicketType + " Ticket",
                                Description = description,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = $"{_configuration["Stripe:SuccessUrl"]}?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = _configuration["Stripe:CancelUrl"],
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            payment.PaymentStatus = "Pending";
            payment.Amount = ticket.Price;
            payment.PaymentIntentId = session.Id;
            _context.Payments.Add(payment);
            ticket.Status = "Not Available";  // Update ticket status immediately
            await _context.SaveChangesAsync();

            return Ok(new { SessionUrl = session.Url });
        }

        [HttpGet("payment-success")]
        public async Task<IActionResult> PaymentSuccess(string session_id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == session_id);
            if (payment == null)
            {
                return NotFound("Payment record not found.");
            }

            payment.PaymentStatus = "Successful";
            var ticket = await _context.Tickets.FindAsync(payment.TicketId);
            if (ticket != null)
            {
                ticket.Status = "Not Available";
            }

            var bookingHistory = new BookingHistory
            {
                UserId = payment.UserId,
                TicketId = payment.TicketId,
                BookingDate = DateTime.UtcNow,
                Status = "Successful"
            };

            _context.BookingHistories.Add(bookingHistory);
            await _context.SaveChangesAsync();

            return Ok("Payment was successful! Thank you for your purchase.");
        }

        [HttpGet("payment-cancel")]
        public async Task<IActionResult> PaymentCancel(string session_id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == session_id);
            if (payment == null)
            {
                return NotFound("Payment record not found.");
            }

            payment.PaymentStatus = "Canceled";
            var ticket = await _context.Tickets.FindAsync(payment.TicketId);
            if (ticket != null)
            {
                ticket.Status = "Available";  // Reset ticket status if canceled
            }

            await _context.SaveChangesAsync();
            return Ok("Payment was canceled. You have not been charged.");
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
