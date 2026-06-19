using Microsoft.EntityFrameworkCore;
using TemplateService.Domain.Departments;

namespace TemplateService.Infrastructure.Postgres;

public class TemplateServiceDbContext : DbContext
{
    private readonly string _connectionString;

    public TemplateServiceDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);

        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateServiceDbContext).Assembly);
    }

    public DbSet<Department> Department => Set<Department>();
}