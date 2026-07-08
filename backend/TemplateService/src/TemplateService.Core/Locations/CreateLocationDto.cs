namespace TemplateService.Core.Locations;

/// <summary>
/// DTO для создания локации.
/// </summary>
/// <param name="Name">Отображаемое название локации.</param>
/// <param name="Street">Улица, дом, корпус.</param>
/// <param name="City">Город.</param>
/// <param name="ZipCode">Почтовый индекс.</param>
/// <param name="Country">Страна.</param>
public sealed record CreateLocationDto(
    string Name,
    string Street,
    string City,
    string? ZipCode,
    string Country
);
