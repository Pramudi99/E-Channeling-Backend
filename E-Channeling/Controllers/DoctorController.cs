using E_Channeling.Data;
using E_Channeling.DTOs.Doctor;
using E_Channeling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace E_Channeling.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DoctorController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Include(d => d.User)
                .Select(d => new
                {
                    d.Id,
                    d.UserId,
                    FullName = d.User.FullName,
                    Email = d.User.Email,
                    d.Specialization,
                    d.Qualification,
                    d.Experience,
                    d.ConsultationFee,
                    d.Description
                })
                .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = "Doctor not found."
                });
            }

            return Ok(new
            {
                doctor.Id,
                doctor.UserId,
                FullName = doctor.User.FullName,
                Email = doctor.User.Email,
                doctor.Specialization,
                doctor.Qualification,
                doctor.Experience,
                doctor.ConsultationFee,
                doctor.Description
            });

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDoctor(
    CreateDoctorRequest request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Email already exists."
                });
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Role = "Doctor"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password
            );

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            var doctor = new Doctor
            {
                UserId = user.Id,
                Specialization = request.Specialization,
                Qualification = request.Qualification,
                Experience = request.Experience,
                ConsultationFee = request.ConsultationFee,
                Description = request.Description
            };

            _context.Doctors.Add(doctor);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Doctor created successfully.",
                doctorId = doctor.Id
            });
        }


    }
}
