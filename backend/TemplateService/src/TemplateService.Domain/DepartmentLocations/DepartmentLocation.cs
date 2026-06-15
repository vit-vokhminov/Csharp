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
    public Guid DepartmentId { get; private set; }

    /// <summary>
    /// Идентификатор локации.
    /// </summary>
    public Guid LocationId { get; private set; }

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
    /// Приватный конструктор для ORM и фабрик.
    /// </summary>
    private DepartmentLocation(
        Guid id,
        Guid departmentId,
        Guid locationId,
        bool isPrimary,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        DepartmentId = departmentId;
        LocationId = locationId;
        IsPrimary = isPrimary;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Создаёт связь подразделения с локацией.
    /// </summary>
    public static DepartmentLocation Create(
        Guid id,
        Guid departmentId,
        Guid locationId,
        bool isPrimary,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("departmentLocation.id.empty", "Идентификатор связи не может быть пустым.");
        }

        if (departmentId == Guid.Empty)
        {
            throw new DomainException("departmentLocation.departmentId.empty", "Идентификатор подразделения не может быть пустым.");
        }

        if (locationId == Guid.Empty)
        {
            throw new DomainException("departmentLocation.locationId.empty", "Идентификатор локации не может быть пустым.");
        }

        // Изначально UpdatedAt совпадает с CreatedAt
        return new DepartmentLocation(
            id,
            departmentId,
            locationId,
            isPrimary,
            createdAt,
            updatedAt: createdAt);
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
