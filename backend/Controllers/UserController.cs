using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using backend.Entities;
using backend.DTO;
using BCryptNet = BCrypt.Net.BCrypt;

namespace backend.Controllers;

[Authorize(Roles = "Admin")] // Only Admins can access ANY route in this controller
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET: api/user
    // Retrieves a list of all users (usually displayed in an Admin data table)
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        // Select only safe data to return (Don't return PasswordHashes!)
        var users = await _context.Users
            .Select(u => new 
            {
                u.Id,
                u.Username,
                // u.Email,
                u.Role,
                // u.IsActive
            })
            .ToListAsync();

        return Ok(users);
    }

    // 2. GET: api/user/{id}
    // Retrieves details for a specific user
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null) return NotFound(new { message = "User not found" });

        // Again, map to a DTO or anonymous object to hide the password
        return Ok(new 
        {
            user.Id,
            user.Username,
            // user.Email,
            user.Role,
            // user.IsActive
        });
    }

    // 3. POST: api/user
    // Admin creates a brand new user
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO dto)
    {
        // Check if email already exists
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
        {
            return BadRequest(new { message = "Email already in use." });
        }

        // Create the new entity
        var newUser = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Role = dto.Role,
            IsActive = true,
            // Use BCrypt to hash the password before saving!
            PasswordHash = BCryptNet.HashPassword(dto.Password, 13) 
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, new { message = "User created successfully", id = newUser.Id });
    }

    // 4. PUT: api/user/{id}
    // Admin updates existing user data
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO dto)
    {
        // 1. Find the user (Starts Change Tracking)
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "User not found" });

        // 2. Update fields safely using ?? (Null-coalescing)
        if(!string.IsNullOrEmpty(dto.Username)) user.Username = dto.Username;
        if(!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
        user.Role = dto.Role ?? user.Role;

        // Note: Password resets are usually handled in a separate endpoint, 
        // but if the Admin provides a new password here, hash and update it.
        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.PasswordHash = BCryptNet.HashPassword(dto.Password);
        }

        // 3. Save Changes (No need for _context.Users.Update(user)!)
        await _context.SaveChangesAsync();

        return Ok(new { message = "User updated successfully" });
    }

    // 5. DELETE: api/user/{id}
    // Admin deletes a user (Soft Delete recommended over Hard Delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "User not found" });

        user.IsActive = false; 

        await _context.SaveChangesAsync();

        return Ok(new { message = "User successfully deactivated/deleted" });
    }
}