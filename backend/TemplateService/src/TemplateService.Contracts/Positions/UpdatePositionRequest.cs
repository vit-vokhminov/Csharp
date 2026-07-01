namespace TemplateService.Contracts.Positions;

/// <summary>
/// Запрос на обновление должности.
/// </summary>
public sealed class UpdatePositionRequest
{
    /// <summary>
    /// Отображаемое название должности.
    /// </summary>
    public required string Name { get; init; }
}