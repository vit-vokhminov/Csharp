using Microsoft.EntityFrameworkCore;
using TemplateService.Domain.DepartmentLocations;
using TemplateService.Domain.DepartmentPositions;
using TemplateService.Domain.Departments;
using TemplateService.Domain.Locations;
using TemplateService.Domain.Positions;
using TemplateService.Infrastructure.Postgres.Configurations;

namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Точка подключения маппинга доменных сущностей к PostgreSQL через EF Core.
/// </summary>
public class TemplateServiceDbContext : DbContext
{
    /// <summary>
    /// Подразделения компании.
    /// </summary>
    public DbSet<Department> Departments => Set<Department>();

    /// <summary>
    /// Локации, в которых работают подразделения.
    /// </summary>
    public DbSet<Location> Locations => Set<Location>();

    /// <summary>
    /// Должности, доступные внутри подразделений.
    /// </summary>
    public DbSet<Position> Positions => Set<Position>();

    /// <summary>
    /// Связи подразделений с локациями.
    /// </summary>
    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();

    /// <summary>
    /// Связи подразделений с должностями.
    /// </summary>
    public DbSet<DepartmentPosition> DepartmentPositions => Set<DepartmentPosition>();

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TemplateServiceDbContext"/>.
    /// </summary>
    /// <param name="options">Опции контекста базы данных.</param>
    public TemplateServiceDbContext(DbContextOptions<TemplateServiceDbContext> options)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("directory");

        // Основные сущности
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());

        // Связи между сущностями
        modelBuilder.ApplyConfiguration(new DepartmentLocationConfiguration());
        modelBuilder.ApplyConfiguration(new DepartmentPositionConfiguration());
    }
}
