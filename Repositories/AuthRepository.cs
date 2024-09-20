using Microsoft.EntityFrameworkCore;
using opms_server_core.Data;
using opms_server_core.Models;
using opms_server_core.Interfaces;
using opms_server_core.Utils;


namespace opms_server_core.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly OPMSDbContext _context;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthRepository(OPMSDbContext context, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResult?> Authenticate(UserLoginRequest request)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || user.PasswordHash != request.Password)
            {
                return null; // Invalid email or password
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResult
            {
                Token = token,
                UserId = user.Id.ToString(),
                Email = user.Email,
                FullName = user.FullName,
                CompanyName = user.CompanyName,
                Description = user.Description,
                Role = user.Role
            };
        }

        public async Task<RegisterResult> Register(CreateUser request)
        {
            var existingUser = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = "A user with this email already exists."
                };
            }

            var newUser = new UserProfile
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CompanyName = request.CompanyName,
                Description = request.Description,
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.UserProfiles.Add(newUser);
            var result = await _context.SaveChangesAsync();

            return new RegisterResult
            {
                Success = result > 0, // Check if rows were affected
                Message = result > 0 ? "Registration completed" : "Registration failed"
            };
        }


    }
}
