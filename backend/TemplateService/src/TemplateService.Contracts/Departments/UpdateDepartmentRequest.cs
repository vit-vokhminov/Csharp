namespace TemplateService.Contracts.Departments;

/// <summary>
/// Запрос на обновление подразделения.
/// </summary>
public sealed class UpdateDepartmentRequest
{
    /// <summary>
    /// Отображаемое название подразделения.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Идентификатор родительского подразделения. Для корневого - null.
    /// </summary>
    public Guid? ParentId { get; init; }
}