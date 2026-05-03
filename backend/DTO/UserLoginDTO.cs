using backend.Entities;

namespace backend.DTO;

public class UserLoginDTO
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginResponseDTO
{
    public string Token { get; set; } = "";
    public User User { get; set; } = new User(); 
}