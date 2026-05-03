using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.Entities;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime HireDate { get; set; }
    public DateTime BirthDate { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
}
