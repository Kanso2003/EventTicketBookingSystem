﻿namespace EventTicketBookingSystem.Models
{
    public class EventOrganizer
    {
        public int OrganizerId { get; set; }  // Primary Key
        public string OrganizerName { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Default value
    }
}
