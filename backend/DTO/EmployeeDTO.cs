using System.ComponentModel.DataAnnotations.Schema;

namespace backend.DTO;

public class EmployeeDTO
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public decimal Salary { get; set; } = 0;
    public string FormattedSalary => $"{Salary:N0}";
    [ForeignKey("Department")]
    public int DepartmentId { get; set; } = 0;
    [ForeignKey("Position")]
    public int PositionId { get; set; } = 0;
}