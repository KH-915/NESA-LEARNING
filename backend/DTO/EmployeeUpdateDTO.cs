using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Enums;

namespace backend.DTO;

public class EmployeeUpdateDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? HireDate { get; set; }
    public DateTime? BirthDate { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Salary { get; set; } = 0;
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public EmployeeStatus? Status { get; set; } = EmployeeStatus.Active;
}