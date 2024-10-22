using EventTicketBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EventTicketBookingSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<EventOrganizer> EventOrganizers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring User and Ticket relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tickets)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuring Event and Ticket relationship
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Tickets)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId);

            // Configuring Ticket and Payment relationship
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Payment)
                .WithOne()
                .HasForeignKey<Payment>(p => p.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring User and Payment relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // Configuring User and BookingHistory relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.BookingHistories)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            // Configuring Ticket and BookingHistory relationship
            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.BookingHistories)
                .WithOne(b => b.Ticket)
                .HasForeignKey(b => b.TicketId);

            // Configure the primary key for BookingHistory
            modelBuilder.Entity<BookingHistory>()
                .HasKey(b => b.BookingId); // Define the primary key

            // Configure the primary key for EventOrganizer
            modelBuilder.Entity<EventOrganizer>()
        .HasKey(eo => eo.OrganizerId); // Define the primary key
        }
    }
}
