namespace TemplateService.Contracts.Positions;

/// <summary>
/// Ответ с данными должности.
/// </summary>
public sealed class PositionResponse
{
    /// <summary>
    /// Идентификатор должности.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название должности.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего изменения записи.
    /// </summary>
    public required DateTime UpdatedAt { get; init; }
}