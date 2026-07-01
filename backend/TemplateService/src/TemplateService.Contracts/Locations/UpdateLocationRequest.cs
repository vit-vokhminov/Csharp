namespace TemplateService.Contracts.Locations;

/// <summary>
/// Запрос на обновление локации.
/// </summary>
public sealed class UpdateLocationRequest
{
    /// <summary>
    /// Отображение названия локации.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Адрес локации.
    /// </summary>
    public required AddressDto Address { get; init; }
}