using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers;

[Authorize(Roles = "Admin, HR")]
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartmentList()
    {
        var DepartmentList = await _context.Departments.ToListAsync();
        return Ok(DepartmentList);
    }

}   