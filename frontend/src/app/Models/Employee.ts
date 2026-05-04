import { Department } from "./Department";
import { Position } from "./Position";

// public int Id { get; set; }
// public string FirstName { get; set; } = "";
// public string LastName { get; set; } = "";
// public string Email { get; set; } = "";
// public string Phone { get; set; } = "";
// public DateTime HireDate { get; set; }
// public DateTime BirthDate { get; set; }
// [Column(TypeName = "decimal(18, 2)")]
// public decimal Salary { get; set; }
// public int DepartmentId { get; set; }
// public int PositionId { get; set; }
// public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

export interface Employee {
    id: number,
    firstname: string,
    lastname: string,
    email: string,
    phone: string,
    hiredate: string,
    birthdate: string,
    salary: number,
    departmentid: number,
    department: Department,
    positionid: number,
    postion: Position,
    employeestatus: number
}