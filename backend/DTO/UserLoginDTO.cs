using backend.Entities;
using backend.Enums;
namespace backend.DTO;

public class UserLoginDTO
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginResponseDTO
{
    public string Token { get; set; } = "";
    public UserResponseDTO User { get; set; } = new UserResponseDTO(); 
}

public class UserResponseDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.Guest;
    public int? EmployeeId { get; set; }
    public bool IsActive { get; set; } = true;
}