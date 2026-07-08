using TemplateService.Domain.Locations;

namespace TemplateService.Core.Locations;

/// <summary>
/// Контракт репозитория для работы с локациями.
/// </summary>
public interface ILocationRepository
{
    /// <summary>
    /// Добавляет новую локацию в хранилище.
    /// </summary>
    /// <param name="location">Локация для добавления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Добавленная локация.</returns>
    Task<Location> AddAsync(Location location, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет, занято ли имя локации.
    /// </summary>
    /// <param name="name">Имя локации для проверки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если имя занято; иначе false.</returns>
    Task<bool> IsNameTakenAsync(string name, CancellationToken cancellationToken = default);
}
