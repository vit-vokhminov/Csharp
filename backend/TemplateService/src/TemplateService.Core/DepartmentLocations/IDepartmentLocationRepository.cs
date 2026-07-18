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
}
