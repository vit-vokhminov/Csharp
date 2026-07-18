namespace TemplateService.Contracts.Departments;

/// <summary>
/// Запрос на создание подразделения.
/// </summary>
public sealed class CreateDepartmentRequest
{
    /// <summary>
    /// Отображаемое название подразделения.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Стабильный код для URL и интеграций.
    /// </summary>
    public required string Slug { get; init; }

    /// <summary>
    /// Идентификатор родительского подразделения. Для корневого - null.
    /// </summary>
    public Guid? ParentId { get; init; }

    /// <summary>
    /// Идентификаторы локаций, где работает подразделение.
    /// </summary>
    public IReadOnlyList<Guid> LocationIds { get; init; } = Array.Empty<Guid>();
}
