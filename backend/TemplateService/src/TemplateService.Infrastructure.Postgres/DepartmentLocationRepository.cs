using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TemplateService.Core.DepartmentLocations;
using TemplateService.Domain.DepartmentLocations;

namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Реализация репозитория связей подразделений с локациями через EF Core.
/// </summary>
public sealed partial class DepartmentLocationRepository : IDepartmentLocationRepository
{
    private readonly TemplateServiceDbContext _dbContext;
    private readonly ILogger<DepartmentLocationRepository> _logger;

    public DepartmentLocationRepository(
        TemplateServiceDbContext dbContext,
        ILogger<DepartmentLocationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<DepartmentLocation> AddAsync(DepartmentLocation departmentLocation, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.DepartmentLocations.AddAsync(departmentLocation, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return departmentLocation;
        }
        catch (Exception ex)
        {
            LogSaveError(ex, departmentLocation.DepartmentId, departmentLocation.LocationId);
            throw new InfrastructureException(
                $"Не удалось сохранить связь подразделения {departmentLocation.DepartmentId} с локацией {departmentLocation.LocationId}",
                ex);
        }
    }

    public async Task<IReadOnlyList<DepartmentLocation>> AddRangeAsync(IReadOnlyList<DepartmentLocation> departmentLocations, CancellationToken cancellationToken = default)
    {
        if (departmentLocations.Count == 0)
        {
            return Array.Empty<DepartmentLocation>();
        }

        try
        {
            await _dbContext.DepartmentLocations.AddRangeAsync(departmentLocations, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return departmentLocations;
        }
        catch (Exception ex)
        {
            LogSaveRangeError(ex, departmentLocations.Count);
            throw new InfrastructureException($"Не удалось сохранить связи подразделений с локациями", ex);
        }
    }

    [LoggerMessage(LogLevel.Error, "Ошибка сохранения связи подразделения с локацией DepartmentId={DepartmentId}, LocationId={LocationId}")]
    private partial void LogSaveError(Exception ex, Guid departmentId, Guid locationId);

    [LoggerMessage(LogLevel.Error, "Ошибка сохранения {Count} связей подразделений с локациями")]
    private partial void LogSaveRangeError(Exception ex, int count);
}