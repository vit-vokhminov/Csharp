using TemplateService.Domain.DepartmentLocations;

namespace TemplateService.Core.DepartmentLocations;

/// <summary>
/// Контракт репозитория для работы со связями подразделений и локаций.
/// </summary>
public interface IDepartmentLocationRepository
{
    /// <summary>
    /// Добавляет новую связь подразделения с локацией.
    /// </summary>
    /// <param name="departmentLocation">Связь для добавления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Добавленная связь.</returns>
    Task<DepartmentLocation> AddAsync(
        DepartmentLocation departmentLocation,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет несколько связей подразделения с локациями.
    /// </summary>
    /// <param name="departmentLocations">Список связей для добавления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Добавленные связи.</returns>
    Task<IReadOnlyList<DepartmentLocation>> AddRangeAsync(
        IReadOnlyList<DepartmentLocation> departmentLocations,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохраняет все изменения в БД.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача выполнения.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет, существует ли связь между подразделением и локацией.
    /// </summary>
    /// <param name="departmentId">Идентификатор подразделения.</param>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если связь существует; иначе false.</returns>
    Task<bool> ExistsAsync(Guid departmentId, Guid locationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Находит связь по идентификаторам подразделения и локации.
    /// </summary>
    /// <param name="departmentId">Идентификатор подразделения.</param>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Связь или null, если не найдена.</returns>
    Task<DepartmentLocation?> GetByDepartmentAndLocationAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет связь между подразделением и локацией.
    /// </summary>
    /// <param name="departmentLocation">Связь для удаления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача выполнения.</returns>
    Task RemoveAsync(DepartmentLocation departmentLocation, CancellationToken cancellationToken = default);

}
