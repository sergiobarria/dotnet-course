using CompanyEmployees.Core.Domain.Entities;
using CompanyEmployees.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Infrastructure.Persistence;

public class RepositoryContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Company>? Companies { get; set; }

    public DbSet<Employee>? Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
}