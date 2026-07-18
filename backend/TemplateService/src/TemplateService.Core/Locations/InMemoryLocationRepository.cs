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
        // На случай, если локация с таким ID уже есть — перезаписываем (или можно выбросить исключение, если нужна строгая семантика)
        _locations[location.Id] = location;
        _nameIndex[location.Name.Value] = location.Id;

        return Task.FromResult(location);
    }

    public Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken = default)
    {
        var isTaken = _nameIndex.ContainsKey(name);
        return Task.FromResult(isTaken);
    }

    // Исправленное имя метода: GetByIdAsync вместо GetBuIdAsync
    public Task<Location?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _locations.TryGetValue(id, out var location);
        return Task.FromResult<Location?>(location);
    }
}
