using FluentValidation;
using TemplateService.Domain.Locations;

namespace TemplateService.Core.Locations;

/// <summary>
/// Сценарий создания локации.
/// </summary>
public sealed class CreateLocationUseCase
{
    private readonly ILocationRepository _locationRepository;
    private readonly CreateLocationDtoValidator _validator;

    public CreateLocationUseCase(
        ILocationRepository locationRepository,
        CreateLocationDtoValidator validator)
    {
        _locationRepository = locationRepository;
        _validator = validator;
    }

    /// <summary>
    /// Выполняет сценарий создания локации.
    /// </summary>
    /// <param name="dto">Данные для создания локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор созданной локации.</returns>
    /// <exception cref="ValidationException">Выбрасывается при невалидных входных данных.</exception>
    /// <exception cref="LocationNameTakenException">Выбрасывается при занятом имени локации.</exception>
    public async Task<Guid> ExecuteAsync(CreateLocationDto dto, CancellationToken cancellationToken = default)
    {
        // Валидация входных данных
        await _validator.ValidateAndThrowAsync(dto, cancellationToken);

        // Проверка уникальности имени
        var isNameTaken = await _locationRepository.IsNameTakenAsync(dto.Name, cancellationToken);
        if (isNameTaken)
        {
            throw new LocationNameTakenException(dto.Name);
        }

        // Создание локации
        var location = Location.Create(
            Guid.NewGuid(),
            dto.Name,
            BuildFullAddress(dto),
            DateTime.UtcNow);

        // Сохранение
        await _locationRepository.AddAsync(location, cancellationToken);

        return location.Id;
    }

    private static string BuildFullAddress(CreateLocationDto dto)
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(dto.Street))
            parts.Add(dto.Street);

        if (!string.IsNullOrWhiteSpace(dto.City))
            parts.Add(dto.City);

        if (!string.IsNullOrWhiteSpace(dto.ZipCode))
            parts.Add(dto.ZipCode);

        if (!string.IsNullOrWhiteSpace(dto.Country))
            parts.Add(dto.Country);

        return string.Join(", ", parts);
    }
}
