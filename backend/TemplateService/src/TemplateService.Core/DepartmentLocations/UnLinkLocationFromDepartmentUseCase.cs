using TemplateService.Core.Departments;
using TemplateService.Domain.DepartmentLocations;

namespace TemplateService.Core.DepartmentLocations;

/// <summary>
/// Use case для отвязки локации от подразделения.
/// </summary>
public sealed class UnlinkLocationFromDepartmentUseCase
{
    private readonly IDepartmentLocationRepository _departmentLocationRepository;

    public UnlinkLocationFromDepartmentUseCase(IDepartmentLocationRepository departmentLocationRepository)
    {
        _departmentLocationRepository = departmentLocationRepository;
    }

    /// <summary>
    /// Отвязывает локацию от подразделения.
    /// Если связи нет, выбрасывает исключение.
    /// </summary>
    /// <param name="departmentId">Идентификатор подразделения.</param>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Удаленная связь.</returns>
    /// <exception cref="DepartmentLocationNotFoundException">Выбрасывается, если связь не найдена.</exception>
    public async Task<DepartmentLocation> ExecuteAsync(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken = default)
    {
        // Находим связь
        var existingLink = await _departmentLocationRepository
            .GetByDepartmentAndLocationAsync(departmentId, locationId, cancellationToken);

        if (existingLink is null)
        {
            throw new DepartmentLocationNotFoundException(departmentId, locationId);
        }

        // Удаляем связь
        await _departmentLocationRepository.RemoveAsync(existingLink, cancellationToken);

        return existingLink;
    }
}

/// <summary>
/// Исключение, выбрасываемое, когда связь подразделения с локацией не найдена.
/// </summary>
public sealed class DepartmentLocationNotFoundException : Exception
{
    public Guid DepartmentId { get; }
    public Guid LocationId { get; }

    public DepartmentLocationNotFoundException()
    {
    }

    public DepartmentLocationNotFoundException(string message)
        : base(message)
    {
    }

    public DepartmentLocationNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DepartmentLocationNotFoundException(Guid departmentId, Guid locationId)
        : base($"Связь подразделения {departmentId} с локацией {locationId} не найдена.")
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }
}
