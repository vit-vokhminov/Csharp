namespace TemplateService.Contracts.Locations;

/// <summary>
/// Адрес локации.
/// </summary>
public sealed class AddressDto
{
    /// <summary>
    /// Улица, дом, корпус.
    /// </summary>
    public required string Street { get; init; }

    /// <summary>
    /// Город.
    /// </summary>
    public required string City { get; init; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    public string? ZipCode { get; init; }

    /// <summary>
    /// Страна.
    /// </summary>
    public required string Country { get; init; }

}