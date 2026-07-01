namespace TemplateService.Contracts.Locations;

/// <summary>
/// Запрос на создание локации.
/// </summary>
public sealed class CreateLocationRequest
{
    /// <summary>
    /// Отображаемое название локации.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Адрес локации.
    /// </summary>
    public required AddressDto Address { get; set; }
}