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
    /// </summary>
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
    /// <param name="id">Идентификатор должности.</param>
    /// <param name="name">Название должности.</param>
    /// <param name="createdAt">Дата создания записи.</param>
    /// <returns>Экземпляр Position.</returns>
    /// <exception cref="DomainException">Выбрасывается, если идентификатор пустой.</exception>
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

        // Изначально UpdatedAt совпадает с CreatedAt
        return new Position(
            id,
            nameVo,
            createdAt,
            updatedAt: createdAt);
    }

    /// <summary>
    /// Переименовывает должность.
    /// </summary>
    /// <param name="newName">Новое название должности.</param>
    /// <param name="updatedAt">Дата последнего изменения.</param>
    public void Rename(string newName, DateTime updatedAt)
    {
        Name = Name.Create(newName);
        UpdatedAt = updatedAt;
    }
}
