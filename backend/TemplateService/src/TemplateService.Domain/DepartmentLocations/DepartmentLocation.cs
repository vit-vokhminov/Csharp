using TemplateService.Domain.Primitives;

namespace TemplateService.Domain.DepartmentLocations;

/// <summary>
/// Связь подразделения с локацией (many-to-many).
/// Описывает, в каких локациях работает подразделение. 
/// </summary>
public class DepartmentLocation
{
    /// <summary>
    /// Идентификатор связи.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Идентификатор подразделения.
    /// </summary>
    public Guid Departmentld { get; private set; }

    /// <summary>
    /// Идентификатор локации.
    /// </summary>
    public Guid Locationld { get; private set; }

    /// <summary>
    /// Признак основной локации подразделения.
    /// </summary>
    public bool IsPrimary { get; private set; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата последнего изменения записи.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Приватный конструктор для ORR и фабрик.
    /// </summary>
    private DepartmentLocation(
    Guid id,
    Guid departmentld,
    Guid locationld, bool isPrimary,
    DateTime createdAt,
    DateTime updatedAt)
    {
        Id = id;
        Departmentld = departmentld;
        Locationld = locationld;
        IsPrimary = isPrimary;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Создаёт связь подразделения с локацией.
    /// </summary>
    public static DepartmentLocation Create(
        Guid id,
        Guid departmentld,
        Guid locationld,
        bool isPrimary,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("departmentLocation.id.empty", "Идентификатор связи не может быть пустым.");
        }

        if (departmentld == Guid.Empty)
        {
            throw new DomainException("departmentLocation.departmentld.empty", "Идентификатор подразделения не может быть пустым.");
        }

        if (locationld == Guid.Empty)
        {
            throw new DomainException("departmentLocation.locationld.empty", "Идентификатор локации не может быть пустым.");
        }

        return new DepartmentLocation(
            id,
            departmentld,
            locationld,
            isPrimary,
            createdAt,
            createdAt);
    }

    /// <summary>
    /// Устанавливает или снимает признак основной локации.
    /// </summary>
    public void SetPrimary(bool isPrimary, DateTime updatedAt)
    {
        IsPrimary = isPrimary;
        UpdatedAt = updatedAt;
    }
}