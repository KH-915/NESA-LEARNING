using Microsoft.EntityFrameworkCore;
using backend.Entities;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Position> Positions => Set<Position>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
        .Property(e => e.Salary)
        .HasPrecision(18, 2);
        modelBuilder.Entity<Position>()
        .Property(e => e.BaseSalary)
        .HasPrecision(18, 2);
    }
}