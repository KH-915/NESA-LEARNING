using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.Guest;
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public bool IsActive { get; set; } = true;
}