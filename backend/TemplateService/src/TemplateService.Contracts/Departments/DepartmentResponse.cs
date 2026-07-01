namespace TemplateService.Contracts.Departments;

/// <summary>
/// Ответ с данными подразделения.
/// </summary>
public sealed class DepartmentResponse
{
    /// <summary>
    /// Идентификатор подразделения.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название подразделения.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Стабильный код для URL и интеграций.
    /// </summary>
    public required string Slug { get; init; }

    /// <summary>
    /// Полный путь в дереве оргструктуры.
    /// </summary>
    public required string Path { get; init; }

    /// <summary>
    /// Идентификатор родительского подразделения. Для корневого - null.
    /// </summary>
    public required Guid? ParentId { get; init; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего изменения записи.
    /// </summary>
    public required DateTime UpdatedAt { get; init; }
}