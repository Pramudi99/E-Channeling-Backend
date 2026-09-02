using E_Channeling.Data;
using E_Channeling.DTOs;
using E_Channeling.Models;
using E_Channeling.Services;
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
            private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
            {
                _context = context;
                _passwordHasher = new PasswordHasher<User>();
                _jwtService = jwtService;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // 1. Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // 2. Verify password
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // 3. Generate JWT
            var token = _jwtService.GenerateToken(user);

            // 4. Return response
            var response = new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(response);
        }

    }
}
