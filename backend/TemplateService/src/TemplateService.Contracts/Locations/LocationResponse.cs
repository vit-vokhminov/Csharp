namespace TemplateService.Contracts.Locations;

/// <summary>
/// Ответ с данными локации.
/// </summary>
public sealed class LocationResponse
{
    /// <summary>
    /// Идентификатор локации.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название локации.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Адрес локации.
    /// </summary>
    public required AddressDto Address { get; init; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего изменения записи.
    /// </summary>
    public required DateTime UpdatedAt { get; init; }
}