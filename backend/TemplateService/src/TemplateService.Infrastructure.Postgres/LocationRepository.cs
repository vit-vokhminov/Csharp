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
        // Сравниваем по внутреннему значению, а не по самому ValueObject
        return await _dbContext.Locations
            .AnyAsync(l => l.Name.Value == name, cancellationToken);
    }

    public async Task<Location?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // FindAsync принимает параметры через params, поэтому просто передаём id
        return await _dbContext.Locations.FindAsync([id], cancellationToken);
    }

    [LoggerMessage(LogLevel.Error, "Ошибка сохранения локации с именем {Name}")]
    private partial void LogSaveError(Exception ex, string name);
}
