using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Entities;
using backend.DTO;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
    {
        string PasswordHash = BCryptNet.HashPassword(userRegisterDTO.Password, 13);
        var user = new User{
            Username = userRegisterDTO.Username,
            PasswordHash = PasswordHash
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }
    private string CreateToken(User user)
    {
        return "A";
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]UserLoginDTO userLoginDTO)
    {
        var user = await _context.Users.FirstOrDefaultAsync(e => e.Username == userLoginDTO.Username);
        if(user == null){
            return Unauthorized("Username Unavailable!");
        }
        if(!BCryptNet.Verify(userLoginDTO.Password, user.PasswordHash)){
            return Unauthorized("Invalid Password, Try Again!");
        }
        var token = CreateToken(user);
        return Ok
        (
            token
        );
    }
}