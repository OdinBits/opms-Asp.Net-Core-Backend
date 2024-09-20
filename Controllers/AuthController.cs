using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using opms_server_core.Interfaces;
using System.Security.Claims;

namespace opms_server_core.Controllers
{
    [Route("api/opms/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Authenticate(request);

            if (result == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = result.Token;
            var userId = result.UserId;
            var email = result.Email;
            var fullName = result.FullName;
            var companyName = result.CompanyName;
            var description = result.Description;
            var role = result.Role;

            // Set the token in an HttpOnly cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true if using HTTPS
                SameSite = SameSiteMode.Strict, // Or SameSiteMode.Lax, based on your needs
                Expires = DateTime.UtcNow.AddHours(1) // Adjust expiration as needed
            };

            Response.Cookies.Append("AuthToken", token, cookieOptions);

            return Ok(new
            {
                message = "Login successful",
                userId,
                email,
                fullName,
                companyName,
                description,
                role
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUser request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Register(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                success = result.Success
            });
        }

        [HttpPost("logout")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult Logout()
        {
            // Extract the userId from the claims in the JWT
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // Adjust the claim type to match your JWT

            if (userId == null)
            {
                return Unauthorized(new { Message = "No valid user ID found." });
            }

            // Clear the AuthToken cookie
            Response.Cookies.Delete("AuthToken");

            return Ok(new
            {
                Message = "Logout successful",
                UserId = userId
            });
        }
    }
}
