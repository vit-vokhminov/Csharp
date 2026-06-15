using TemplateService.Domain.Primitives;

namespace TemplateService.Domain.DepartmentPositions;

/// <summary>
/// Связь подразделения с должностью (many-to-many).
/// Описывает, какие должности доступны в подразделении.
/// </summary>
public class DepartmentPosition
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
    /// Идентификатор должности.
    /// </summary>
    public Guid Positionld { get; private set; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Дата последнего изменения записи.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Приватный конструктор для ORM и фабрик
    /// </summary>
    private DepartmentPosition(
        Guid id,
        Guid departmentld,
        Guid positionld,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Departmentld = departmentld;
        Positionld = positionld;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Создаёт связь подразделения с должностью.
    /// </summary>
    public static DepartmentPosition Create(
    Guid id,
    Guid departmentld,
    Guid positionld,
    DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("departmentPosition.id.empty", "Идентификатор связи не может быть пустым.");
        }

        if (departmentld == Guid.Empty)
        {
            throw new DomainException("departmentPosition.departmentld.empty", "Идентификатор подразделения не может быть пустым.");
        }

        if (positionld == Guid.Empty)
        {
            throw new DomainException("departmentPosition.positionld.empty", "Идентификатор должности не может быть пустым.");
        }

        return new DepartmentPosition(
            id,
            departmentld,
            positionld,
            createdAt,
            createdAt);
    }
}