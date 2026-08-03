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

    /// <summary>
    /// Находит локацию по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Локация или null, если не найдена.</returns>
    Task<Location?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Находит локацию по списку идентификаторов.
    /// </summary>
    /// <param name="ids">Список идентификаторов локаций.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список найденных локаций.</returns>
    Task<IReadOnlyList<Location>> GetByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет существующую локацию.
    /// </summary>
    /// <param name="location">Локация для обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача выполнения.</returns>
    Task UpdateAsync(Location location, CancellationToken cancellationToken = default);
}
