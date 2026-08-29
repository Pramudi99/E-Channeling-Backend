using E_Channeling.Data;
using E_Channeling.Models;
using E_Channeling.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Channeling.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly PasswordHasher<User> _passwordHasher;

            public AuthController(ApplicationDbContext context)
            {
                _context = context;
                _passwordHasher = new PasswordHasher<User>();
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterRequest request)
            {
                // 1. Check whether email already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        message = "Email already exists."
                    });
                }

                // 2. Create user
                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Role = "Patient"
                };

                // 3. Hash password
                user.PasswordHash =
                    _passwordHasher.HashPassword(
                        user,
                        request.Password
                    );

                // 4. Add user to database
                _context.Users.Add(user);

                // 5. Save changes
                await _context.SaveChangesAsync();

                // 6. Return response
                return Ok(new
                {
                    message = "Registration successful."
                });
            }
        
        }
}
