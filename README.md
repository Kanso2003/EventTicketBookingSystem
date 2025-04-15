Event Ticket Booking System (ASP.NET Core API)
Project Overview
This project is a fully functional backend API for an Event Ticket Booking System built using ASP.NET Core.
It was designed to handle the core functionality of an event booking platform including:

User registration & login (JWT secured)

Role-based authentication (Admin / User)

Event management (CRUD Operations)

Ticket management (CRUD Operations)

Booking history tracking

PostgreSQL database integration with migrations

API security and clean architecture practices

This project was built entirely from scratch using Visual Studio 2022 for backend development and PostgreSQL as the main relational database.

Features
Secure user authentication using JWT tokens

Password hashing using BCrypt

Admin authorization to restrict access to sensitive APIs

Event management system:

Create / Update / Delete / Get Events

Ticket management system:

Manage Ticket Types, Prices, and Availability

Booking History management:

Track user's past bookings and events

Database Migrations:

Used Entity Framework Migrations for version control and schema updates

PostgreSQL Database Integration using PGAdmin4

Clean Code Structure with Controllers, Models, and Migrations separated

API Testing using .http files in Visual Studio

Technologies Used

Technology	Purpose
ASP.NET Core Web API	Backend Development
PostgreSQL	Relational Database Management
Entity Framework Core	ORM & Migrations
PGAdmin 4	Database Management GUI
C#	Main Programming Language
JWT	Authentication & Token Management
BCrypt	Password Hashing
Visual Studio 2022	Development Environment
Database Schema
Entities & Relationships:

Users:

UserID, Username, Email, PasswordHash, Role

One-to-Many relation with Bookings & Payments

Events:

EventID, Title, Description, Location, Date, MaxCapacity

Tickets:

TicketID, Type, Price, Availability

Bookings:

BookingID, UserID (FK), EventID (FK), TicketID (FK), Date

Payments:

PaymentID, BookingID (FK), Status, Amount

Project Structure:
EventTicketBookingSystem/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── BookingHistoryController.cs
│   ├── EventOrganizerController.cs
│   ├── EventsController.cs
│   ├── PaymentsController.cs
│   ├── TicketsController.cs
│   └── UserController.cs
│
├── Models/
│   ├── ApplicationDbContext.cs
│   ├── BookingHistory.cs
│   ├── Events.cs
│   ├── EventOrganizer.cs
│   ├── LoginRequest.cs
│   ├── Payment.cs
│   ├── Ticket.cs
│   └── User.cs
│
├── Migrations/       # Database Migration Files
│
├── appsettings.json  # PostgreSQL Connection String
│
├── Program.cs        # Project Entry Point
│
└── EventTicketBookingSystem.http # API Testing Requests

Installation & Run Instructions
Clone the repository:

bash
Copy code
git clone https://github.com/Kanso2003/EventTicketBookingSystem.git
Update your appsettings.json file with your PostgreSQL connection string.

Apply the latest database migrations:

bash
Copy code
Update-Database
Run the project:

bash
Copy code
dotnet run
Final Notes
This project showcases my backend development skills, database management experience, and practical understanding of API security best practices.

Built with real-world application structure in mind — ready for production scaling with additional features like payment gateways and frontend integration.

