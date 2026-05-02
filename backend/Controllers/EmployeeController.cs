using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Entities;
using backend.DTO;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

[Authorize(Roles = "Admin, HR")]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }

    // Create
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeDTO employeeDTO)
    {
        var existedDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == employeeDTO.DepartmentId);
        var existedPosition = await _context.Positions.FirstOrDefaultAsync(p => p.Id == employeeDTO.PositionId);

        if(existedDepartment == null){
            return NotFound("Department Id not found!");
        }
        if(existedPosition == null){
            return NotFound("Position Id not found!");
        }
        var Salary = employeeDTO.Salary;
        if(Salary == 0){
            Salary = existedPosition.BaseSalary;
        }

        Employee employee = new(){
            FirstName = employeeDTO.FirstName,
            LastName = employeeDTO.LastName,
            Salary = Salary,
            DepartmentId = employeeDTO.DepartmentId,
            PositionId = employeeDTO.PositionId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Ok(employee);
    }

    // Read
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if(employee == null){
            return NotFound("Employee Not Found!");
        }
        EmployeeDTO employeeDTO = new()
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary,
            DepartmentId = employee.DepartmentId,
            PositionId = employee.PositionId
        };
        return Ok(employeeDTO);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeList()
    {
        var employees = await _context.Employees.ToListAsync();

        return Ok(employees);
    }

    [HttpGet("/department/{id}")]
    public async Task<IActionResult> GetEmployeeByDepartment(int id)
    {
        var employees = await _context.Employees
            .Include(e => e.DepartmentId == id)
            .ToListAsync();
        if (employees == null)
        {
            return NotFound("Employee List with Corresponding Department Id Not Found!");
        }
        return Ok(employees);
    }

    // Update
    public async Task<IActionResult> UpdateEmployee(int Id, EmployeeUpdateDTO employeeUpdateDTO)
    {
        var existedEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == Id);

        if(existedEmployee == null){
            return NotFound("Employee Not Found!");
        }
        
        existedEmployee.FirstName = employeeUpdateDTO.FirstName ?? existedEmployee.FirstName;
        existedEmployee.LastName = employeeUpdateDTO.LastName ?? existedEmployee.LastName;
        existedEmployee.BirthDate = employeeUpdateDTO.BirthDate ?? existedEmployee.BirthDate;
        existedEmployee.Salary = employeeUpdateDTO.Salary ?? existedEmployee.Salary;
        existedEmployee.DepartmentId = employeeUpdateDTO.DepartmentId ?? existedEmployee.DepartmentId;
        existedEmployee.PositionId = employeeUpdateDTO.PositionId ?? existedEmployee.PositionId; 

        await _context.SaveChangesAsync();
        return Ok(existedEmployee);
    }
    
}   