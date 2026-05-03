using backend.Enums;
namespace backend.DTO;

public class UserCreateDTO
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.Guest;
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
}