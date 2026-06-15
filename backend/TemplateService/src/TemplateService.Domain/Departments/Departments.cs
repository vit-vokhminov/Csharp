using TemplateService.Domain.Primitives;

namespace TemplateService.Domain.Departments;

/// <summary>
/// Подразделение компании. Образует иерархию через parent-child связь.
/// </summary>
public class Department
{
    /// <summary>
    /// Идентификатор подразделения.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Отображаемое название подразделения.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Стабильный код для URL, интеграций и дерева.
    /// </summary>
    public Slug Slug { get; private set; }

    /// <summary>
    /// Полный путь в дереве оргструктуры, собранный из slug.
    /// </summary>
    public TreePath Path { get; private set; }

    /// <summary>
    /// Идентификатор родительского подразделения. Для корневого - null.
    /// </summary>
    public Guid? ParentId { get; private set; }

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
    private Department(
        Guid id,
        Name name,
        Slug slug,
        TreePath path,
        Guid? parentId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Path = path;
        ParentId = parentId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Создаёт корневое подразделение (без родителя).
    /// </summary>
    public static Department CreateRoot(
        Guid id,
        string name,
        string slug,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("department.id.empty", "Идентификатор подразделения не может быть пустым.");
        }

        var nameValue = Name.Create(name);
        var slugValue = Slug.Create(slug);
        var path = TreePath.CreateRoot(slugValue);

        // Изначально UpdatedAt совпадает с CreatedAt
        return new Department(
            id,
            nameValue,
            slugValue,
            path,
            null,
            createdAt,
            updatedAt: createdAt);
    }

    /// <summary>
    /// Создаёт дочернее подразделение.
    /// </summary>
    public static Department CreateChild(
        Guid id,
        string name,
        string slug,
        Guid parentId,
        TreePath parentPath,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("department.id.empty", "Идентификатор подразделения не может быть пустым.");
        }

        if (parentId == Guid.Empty)
        {
            throw new DomainException("department.parentId.empty", "Идентификатор родительского подразделения не может быть пустым.");
        }

        var nameValue = Name.Create(name);
        var slugValue = Slug.Create(slug);
        var path = TreePath.CreateChild(parentPath, slugValue);

        // Изначально UpdatedAt совпадает с CreatedAt
        return new Department(
            id,
            nameValue,
            slugValue,
            path,
            parentId,
            createdAt,
            updatedAt: createdAt);
    }

    /// <summary>
    /// Переименовывает подразделение. Slug и Path остаются неизменными.
    /// </summary>
    public void Rename(string newName, DateTime updatedAt)
    {
        Name = Name.Create(newName);
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Обновляет путь подразделения. Используется при перемещении в дереве.
    /// </summary>
    public void UpdatePath(TreePath newPath, Guid? newParentId, DateTime updatedAt)
    {
        if (newParentId.HasValue && newParentId.Value == Guid.Empty)
        {
            throw new DomainException("department.parentId.empty", "Идентификатор родительского подразделения не может быть пустым.");
        }

        Path = newPath;
        ParentId = newParentId;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Проверяет, является ли данное подразделение предком другого по пути.
    /// </summary>
    public bool IsAncestorOf(Department other)
    {
        return other.Path.Value.StartsWith(Path.Value + "/", StringComparison.Ordinal);
    }

    /// <summary>
    /// Проверяет, является ли данное подразделение потомком другого по пути.
    /// </summary>
    public bool IsDescendantOf(Department other)
    {
        return Path.Value.StartsWith(other.Path.Value + "/", StringComparison.Ordinal);
    }
}
