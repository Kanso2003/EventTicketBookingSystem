using EventTicketBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context; // Add this line

    public AuthController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context; // Assign the context
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        // Fetch user from the database
        var storedUser = GetUserByUsername(user.Username); // Use the method we are going to define

        if (storedUser == null)
            return Unauthorized(); // Return 401 if user not found

        // Validate password - assuming user.PasswordHash is the input password
        if (VerifyPassword(user.PasswordHash, storedUser.PasswordHash)) // Compare with stored hash
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, storedUser.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) }); // Return the token
        }

        return Unauthorized(); // Return 401 if credentials are invalid
    }

    // Method to fetch user by username
    private User GetUserByUsername(string username)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username); // Query the database
    }

    // Method to verify the password
    private bool VerifyPassword(string password, string storedHash)
    {
        // Use a hashing algorithm to verify the password, e.g., using BCrypt
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
}
