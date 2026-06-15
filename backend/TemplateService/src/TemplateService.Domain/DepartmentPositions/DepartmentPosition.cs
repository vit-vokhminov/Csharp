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
    public Guid DepartmentId { get; private set; }

    /// <summary>
    /// Идентификатор должности.
    /// </summary>
    public Guid PositionId { get; private set; }

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
        Guid departmentId,
        Guid positionId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        DepartmentId = departmentId;
        PositionId = positionId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Создаёт связь подразделения с должностью.
    /// </summary>
    public static DepartmentPosition Create(
        Guid id,
        Guid departmentId,
        Guid positionId,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("departmentPosition.id.empty", "Идентификатор связи не может быть пустым.");
        }

        if (departmentId == Guid.Empty)
        {
            throw new DomainException("departmentPosition.departmentId.empty", "Идентификатор подразделения не может быть пустым.");
        }

        if (positionId == Guid.Empty)
        {
            throw new DomainException("departmentPosition.positionId.empty", "Идентификатор должности не может быть пустым.");
        }

        // Изначально UpdatedAt совпадает с CreatedAt
        return new DepartmentPosition(
            id,
            departmentId,
            positionId,
            createdAt,
            updatedAt: createdAt);
    }

    /// <summary>
    /// Обновляет временную метку последнего изменения.
    /// Используется при изменении связи.
    /// </summary>
    public void UpdateTimestamp(DateTime updatedAt)
    {
        UpdatedAt = updatedAt;
    }
}
