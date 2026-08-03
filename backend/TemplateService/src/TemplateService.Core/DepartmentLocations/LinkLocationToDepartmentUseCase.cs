using TemplateService.Core.Departments;
using TemplateService.Core.Locations;
using TemplateService.Domain.DepartmentLocations;

namespace TemplateService.Core.DepartmentLocations;

/// <summary>
/// Use case для привязки локации к подразделению.
/// </summary>
public sealed class LinkLocationToDepartmentUseCase
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IDepartmentLocationRepository _departmentLocationRepository;

    public LinkLocationToDepartmentUseCase(
        IDepartmentRepository departmentRepository,
        ILocationRepository locationRepository,
        IDepartmentLocationRepository departmentLocationRepository)
    {
        _departmentRepository = departmentRepository;
        _locationRepository = locationRepository;
        _departmentLocationRepository = departmentLocationRepository;
    }

    /// <summary>
    /// Привязывает локацию к подразделению.
    /// Проверяет, что подразделение и локация существуют, и связи ещё нет.
    /// </summary>
    /// <param name="departmentId">Идентификатор подразделения.</param>
    /// <param name="locationId">Идентификатор локации.</param>
    /// <param name="isPrimary">Признак основной локации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданная связь.</returns>
    /// <exception cref="DepartmentNotFoundException">Выбрасывается, если подразделение не найдено.</exception>
    /// <exception cref="LocationNotFoundException">Выбрасывается, если локация не найдена.</exception>
    /// <exception cref="DepartmentLocationAlreadyExistsException">Выбрасывается, если связь уже существует.</exception> 
    public async Task<DepartmentLocation> ExecuteAsync(
        Guid departmentId,
        Guid locationId,
        bool isPrimary,
        CancellationToken cancellationToken = default)
    {
        // Проверяем, что подразделение существует
        var department = await _departmentRepository.GetByIdAsync(departmentId, cancellationToken);
        if (department is null)
        {
            throw new DepartmentNotFoundException(departmentId);
        }

        // Проверяем, что локация существует
        var location = await _locationRepository.GetByIdAsync(locationId, cancellationToken);
        if (location is null)
        {
            throw new LocationNotFoundException(locationId);
        }

        // Проверяем, что связи ещё нет
        var existingLink = await _departmentLocationRepository
            .GetByDepartmentAndLocationAsync(departmentId, locationId, cancellationToken);

        if (existingLink is not null)
        {
            throw new DepartmentLocationAlreadyExistsException(departmentId, locationId);
        }

        // Создаём и сохраняем связь 
        var now = DateTime.UtcNow;
        var departmentLocation = DepartmentLocation.Create(
            Guid.NewGuid(),
            departmentId,
            locationId,
            isPrimary,
            now);

        await _departmentLocationRepository.AddAsync(departmentLocation, cancellationToken);
        await _departmentLocationRepository.SaveChangesAsync(cancellationToken);

        return departmentLocation;
    }
}

/// <summary>
/// Исключение, выбрасываемое, когда связь подразделения с локацией уже существует.
/// </summary>
public sealed class DepartmentLocationAlreadyExistsException : Exception
{
    public Guid DepartmentId { get; }
    public Guid LocationId { get; }

    public DepartmentLocationAlreadyExistsException()
    {
    }

    public DepartmentLocationAlreadyExistsException(string message)
        : base(message)
    {
    }

    public DepartmentLocationAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DepartmentLocationAlreadyExistsException(Guid departmentId, Guid locationId)
        : base($"Связь подразделения {departmentId} с локацией {locationId} уже существует.")
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }
}
