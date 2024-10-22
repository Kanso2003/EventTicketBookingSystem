using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventTicketBookingSystem.Models;

namespace EventTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create transaction record
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Transaction data is required.");
            }

            // Check if the PaymentId exists
            var paymentExists = await _context.Payments.FindAsync(transaction.PaymentId);
            if (paymentExists == null)
            {
                return NotFound("Payment not found.");
            }

            // Add the transaction to the database
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateTransaction), new { id = transaction.TransactionId }, transaction);
        }

        // Read transaction details
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Payment) // Include related payment
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound(new { message = "Transaction not found." });
            }

            return Ok(transaction);
        }

        // Get transaction history for a user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTransactionHistory(int userId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Payment)
                .Where(t => t.Payment.UserId == userId) // Assuming Payment has UserId
                .ToListAsync();

            return Ok(transactions);
        }
    }
}
