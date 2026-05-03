using backend.Enums;

namespace backend.DTO;

public class UserUpdateDTO
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public UserRole? Role { get; set; }
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
}