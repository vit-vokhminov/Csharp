using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TemplateService.Core.Locations;
using TemplateService.Domain.Locations;

namespace TemplateService.Infrastructure.Postgres;

/// <summary>
/// Реализация репозитория локаций через EF Core.
/// </summary>
public sealed partial class LocationRepository : ILocationRepository
{
    private readonly TemplateServiceDbContext _dbContext;
    private readonly ILogger<LocationRepository> _logger;

    public LocationRepository(
        TemplateServiceDbContext dbContext,
        ILogger<LocationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Location> AddAsync(Location location, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Locations.AddAsync(location, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return location;
        }
        catch (Exception ex)
        {
            LogSaveError(ex, location.Name.Value);
            throw new InfrastructureException($"Не удалось сохранить локацию '{location.Name.Value}'", ex);
        }
    }

    public async Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken = default)
    {
        var nameValueObject = Domain.Primitives.Name.Create(name);
        return await _dbContext.Locations
            .AnyAsync(l => l.Name == nameValueObject, cancellationToken);
    }

    public async Task<Location?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // FindAsync принимает параметры через params, поэтому просто передаём id
        return await _dbContext.Locations.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<Location>> GetByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
        {
            return Array.Empty<Location>();
        }

        var locations = await _dbContext.Locations
            .Where(l => ids.Contains(l.Id))
            .ToListAsync(cancellationToken);

        return locations;
    }

    public async Task UpdateAsync(Location location, CancellationToken cancellationToken = default)
    {
        try
        {
            _dbContext.Locations.Update(location);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            LogUpdateError(ex, location.Name.Value);
            throw new InfrastructureException($"He удалось обновить локацию '{location.Name.Value}'", ex);

        }
    }

    [LoggerMessage(LogLevel.Error, "Ошибка сохранения локации с именем {Name}")]
    private partial void LogSaveError(Exception ex, string name);

    [LoggerMessage(LogLevel.Error, "Ошибка обновления локации с именем {Name}")]
    private partial void LogUpdateError(Exception ex, string name);
}
