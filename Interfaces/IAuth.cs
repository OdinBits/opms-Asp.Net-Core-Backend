
namespace opms_server_core.Interfaces
{
    // Define the AuthenticationResult class
    public class UserLoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class RegisterResult
    {
        public bool Success { get; set; }  // Indicates success of the operation
        public string Message { get; set; }  // Message for frontend
    }

    public class LoginResult
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
        public string? Role { get; set; }
    }


    public class CreateUser
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
    }

    // Define the ILoginService interface
    public interface IAuth
    {
        Task<LoginResult> Authenticate(UserLoginRequest request);
        Task<RegisterResult> Register(CreateUser request);
    }
}
