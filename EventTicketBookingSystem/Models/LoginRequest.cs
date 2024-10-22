using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventTicketBookingSystem.Models
{
    /*[Route("api/[controller]")]
    [ApiController]
    public class LoginRequest : ControllerBase
    {
        public string Username { get; set; }
        public string Password { get; set; } // Plain password, not hashed
    }*/
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
