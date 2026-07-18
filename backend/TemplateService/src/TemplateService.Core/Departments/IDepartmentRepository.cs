using TemplateService.Domain.Departments;

namespace TemplateService.Core.Departments;

/// <summary>
/// Контракт репозитория для работы с подразделениями.
/// </summary>
public interface IDepartmentRepository
{
    /// <summary>
    /// Добавляет новое подразделение в хранилище.
    /// </summary>
    /// <param name="department">Подразделение для добавления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Добавленное подразделение.</returns>
    Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет, существует ли подразделение с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор подразделения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если подразделение существует; иначе false.</returns>
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Находит подразделение по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор подразделения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Подразделение или null, если не найдено.</returns>
    Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет, занят ли slug.
    /// </summary>
    /// <param name="slug">Slug для проверки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>True, если slug занят; иначе false.</returns>
    Task<bool> IsSlugTakenAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохраняет все изменения в БД.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача выполнения.</returns>
    Task SaveChangeAsync(CancellationToken cancellationToken = default);
}
