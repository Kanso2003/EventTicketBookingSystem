using EventTicketBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {// User and Ticket relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tickets)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Adjust as needed for your deletion policy.

            // Event and Ticket relationship
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Tickets)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId);

            // Configuring the one-to-many relationship between Ticket and Payment
            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.Payments)
                .WithOne(p => p.Ticket)
                .HasForeignKey(p => p.TicketId);

            // User and Payment relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Payments)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // User and BookingHistory relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.BookingHistories)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            // Primary key configurations
            modelBuilder.Entity<BookingHistory>().HasKey(b => b.BookingId);
            modelBuilder.Entity<EventOrganizer>().HasKey(eo => eo.OrganizerId);

            // Event and EventOrganizer foreign key relationship
            // Keeps OrganizerId as foreign key without navigation property if needed
            modelBuilder.Entity<Event>()
                .HasOne<EventOrganizer>()
                .WithMany()
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict); // No cascade delete.

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            optionsBuilder.EnableSensitiveDataLogging(); // Turn off in production for security reasons.
        }
    }
}
