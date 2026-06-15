using TemplateService.Domain.Primitives;

namespace TemplateService.Domain.Positions;

/// <summary>
/// Должность - роль, доступная внутри подразделений.
/// </summary>
public class Position
{
    /// <summary>
    /// Идентификатор должности.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Отображаемое название должности.
    /// </summary>
    public Name Name { get; private set; }

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
    /// </summary> ВИнИИдЙИН
    private Position(
        Guid id,
        Name name,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Создаёт новую должность.
    /// </summary>
    public static Position Create(
        Guid id,
        string name,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("position.id.empty", "Идентификатор должности не может быть пустым.");
        }

        var nameVo = Name.Create(name);

        return new Position(
            id,
            nameVo,
            createdAt,
            createdAt);
    }

    /// <summary>
    /// Переименовывает должность.
    /// </summary>
    public void Rename(string newName, DateTime updatedAt)
    {
        Name = Name.Create(newName);
        UpdatedAt = updatedAt;
    }
}
