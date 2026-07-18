using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TemplateService.Core.Departments;
using TemplateService.Domain.Departments;

namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Реализация репозитория подразделений через EF Core.
/// </summary>
public sealed partial class DepartmentRepository : IDepartmentRepository
{
    private readonly TemplateServiceDbContext _dbContext;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(
        TemplateServiceDbContext dbContext,
        ILogger<DepartmentRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Departments.AddAsync(department, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return department;
        }
        catch (Exception ex)
        {
            LogSaveError(ex, department.Name.Value);
            throw new InfrastructureException($"Не удалось сохранить подразделение '{department.Name.Value}'", ex);
        }
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments
            .AnyAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // FindAsync принимает параметры через params, поэтому передаём просто id
        return await _dbContext.Departments.FindAsync([id], cancellationToken);
    }

    public async Task<bool> IsSlugTakenAsync(string slug, CancellationToken cancellationToken = default)
    {
        // Сравниваем по внутреннему значению, а не по самому ValueObject
        return await _dbContext.Departments
            .AnyAsync(d => d.Slug.Value == slug, cancellationToken);
    }

    [LoggerMessage(LogLevel.Error, "Ошибка сохранения подразделения с именем {Name}")]
    private partial void LogSaveError(Exception ex, string name);
}