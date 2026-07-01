namespace TemplateService.Contracts.Positions;

/// <summary>
/// Запрос на создание должности.
/// </summary>
public sealed class CreatePositionRequest
{
    /// <summary>
    /// Отображаемое название должности.
    /// </summary>
    public required string Name { get; init; }
}