using TemplateService.Core.Departments;
using TemplateService.Domain.Locations;

namespace TemplateService.Core.Locations;

/// <summary>
/// Use case для обновления существующей локации.
/// </summary>
public sealed class UpdateLocationUseCase
{
    private readonly ILocationRepository _locationRepository;

    public UpdateLocationUseCase(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    /// <summary>
    /// Обновляет название и адрес локации.
    /// </summary>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <param name="newName">Новое название локации.</param>
    /// <param name="newAddress">Новый адрес локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновлённая локация.</returns>
    /// <exception cref="LocationNotFoundException">Выбрасывается, если локация не найдена.</exception>
    public async Task<Location> ExecuteAsync(
        Guid locationId,
        string newName,
        string newAddress,
        CancellationToken cancellationToken = default)
    {
        var location = await _locationRepository.GetByIdAsync(locationId, cancellationToken);
        if (location is null)
        {
            throw new LocationNotFoundException(locationId);
        }

        // Вызываем доменные методы для обновления имени и адреса 
        location.Rename(newName, DateTime.UtcNow);
        location.UpdateAddress(newAddress, DateTime.UtcNow);

        await _locationRepository.UpdateAsync(location, cancellationToken);
        return location;
    }
}
