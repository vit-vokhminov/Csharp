using TemplateService.Contracts.Departments;
using TemplateService.Core.DepartmentLocations;
using TemplateService.Core.Locations;
using TemplateService.Domain.DepartmentLocations;
using TemplateService.Domain.Departments;
using TemplateService.Domain.Locations;

namespace TemplateService.Core.Departments;

/// <summary>
/// Use case для создания нового подразделения.
/// </summary>
public sealed class CreateDepartmentUseCase
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentLocationRepository _departmentLocationRepository;
    private readonly ILocationRepository _locationRepository;

    public CreateDepartmentUseCase(
        IDepartmentRepository departmentRepository,
        IDepartmentLocationRepository departmentLocationRepository,
        ILocationRepository locationRepository)
    {
        _departmentRepository = departmentRepository;
        _departmentLocationRepository = departmentLocationRepository;
        _locationRepository = locationRepository;
    }

    /// <summary>
    /// Создаёт новое подразделение с указанными локациями.
    /// </summary>
    /// <param name="request">Данные для создания подразделения.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Созданное подразделение.</returns>
    /// <exception cref="DepartmentNotFoundException">Выбрасывается, если родительское подразделение не найдено.</exception>
    /// <exception cref="LocationNotFoundException">Выбрасывается, если одна из локаций не найдена.</exception>
    /// <exception cref="SlugAlreadyTakenException">Выбрасывается, если slug уже занят.</exception>
    public async Task<Department> ExecuteAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        // Проверка: slug не должен быть занят
        if (await _departmentRepository.IsSlugTakenAsync(request.Slug, cancellationToken))
        {
            throw new SlugAlreadyTakenException(request.Slug);
        }

        // Проверка родителя: если ParentId указан, родитель должен существовать
        Department? parentDepartment = null;
        if (request.ParentId.HasValue)
        {
            parentDepartment = await _departmentRepository.GetByIdAsync(request.ParentId.Value, cancellationToken);
            if (parentDepartment is null)
            {
                throw new DepartmentNotFoundException(request.ParentId.Value);
            }
        }

        // Проверка локаций: все LocationIds должны существовать
        var locations = new List<Location>();
        if (request.LocationIds.Count > 0)
        {
            foreach (var locationId in request.LocationIds)
            {
                var location = await _locationRepository.GetByIdAsync(locationId, cancellationToken);
                if (location is null)
                {
                    throw new LocationNotFoundException(locationId);
                }

                locations.Add(location);
            }
        }

        // Создание подразделения через доменную фабрику
        var now = DateTime.UtcNow;
        var newDepartment = parentDepartment is null
            ? Department.CreateRoot(Guid.NewGuid(), request.Name, request.Slug, now)
            : Department.CreateChild(Guid.NewGuid(), request.Name, request.Slug, parentDepartment.Id, parentDepartment.Path, now);

        // Сохранение подразделения
        await _departmentRepository.AddAsync(newDepartment, cancellationToken);

        // Создание и сохранение связей с локациями
        if (locations.Count > 0)
        {
            var departmentLocations = new List<DepartmentLocation>();
            for (int i = 0; i < locations.Count; i++)
            {
                var departmentLocation = DepartmentLocation.Create(
                    Guid.NewGuid(),
                    newDepartment.Id,
                    locations[i].Id,
                    isPrimary: i == 0, // Первая локация считается основной
                    now);

                departmentLocations.Add(departmentLocation);
            }

            await _departmentLocationRepository.AddRangeAsync(departmentLocations, cancellationToken);
        }

        // Атомарное сохранение: подразделение и все связи сохраняются одним коммитом
        await _departmentRepository.SaveChangeAsync(cancellationToken);

        return newDepartment;
    }
}

/// <summary>
/// Исключение, выбрасываемое, когда родительское подразделение не найдено.
/// </summary>
public sealed class DepartmentNotFoundException : Exception
{
    public Guid DepartmentId { get; }

    public DepartmentNotFoundException()
    {
    }

    public DepartmentNotFoundException(string message)
        : base(message)
    {
    }

    public DepartmentNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DepartmentNotFoundException(Guid departmentId)
        : base($"Подразделение с идентификатором {departmentId} не найдено.")
    {
        DepartmentId = departmentId;
    }
}

/// <summary>
/// Исключение, выбрасываемое, когда локация не найдена.
/// </summary>
public sealed class LocationNotFoundException : Exception
{
    public Guid LocationId { get; }

    public LocationNotFoundException()
    {
    }

    public LocationNotFoundException(string message)
        : base(message)
    {
    }

    public LocationNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public LocationNotFoundException(Guid locationId)
        : base($"Локация с идентификатором {locationId} не найдена.")
    {
        LocationId = locationId;
    }
}

/// <summary>
/// Исключение, выбрасываемое, когда slug уже занят.
/// </summary>
public sealed class SlugAlreadyTakenException : Exception
{
    public string? Slug { get; }

    public SlugAlreadyTakenException()
    {
    }

    public SlugAlreadyTakenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public SlugAlreadyTakenException(string slug)
        : base($"Slug '{slug}' уже занят.")
    {
        Slug = slug;
    }
}
