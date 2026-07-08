using System.Collections.Concurrent;
using TemplateService.Domain.Locations;

namespace TemplateService.Core.Locations;

/// <summary>
/// Временная заглушка репозитория для тестирования.
/// Реализация будет добавлена в инфраструктурном слое позже.
/// </summary>
public sealed class InMemoryLocationRepository : ILocationRepository
{
    private readonly ConcurrentDictionary<Guid, Location> _locations = new();
    private readonly ConcurrentDictionary<string, Guid> _nameIndex = new(StringComparer.OrdinalIgnoreCase);

    public Task<Location> AddAsync(Location location, CancellationToken cancellationToken = default)
    {
        _locations[location.Id] = location;
        _nameIndex[location.Name.Value] = location.Id;

        return Task.FromResult(location);
    }

    public Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken = default)
    {
        var isTaken = _nameIndex.ContainsKey(name);
        return Task.FromResult(isTaken);
    }
}

