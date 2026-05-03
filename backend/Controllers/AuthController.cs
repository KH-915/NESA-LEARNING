using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BCryptNet = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using backend.Entities;
using backend.Data;
using backend.DTO;
using System.Security.AccessControl;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
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
        var result = new LoginResponseDTO(){
            Token = token,
            User = user
        };
        return Ok(result);
    }

    private string CreateToken(User user)
    {
        var JwtKey = _config["Jwt:Key"];

        if(string.IsNullOrEmpty(JwtKey)){
            throw new Exception("Missing JWT Key!");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptors = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptors);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}