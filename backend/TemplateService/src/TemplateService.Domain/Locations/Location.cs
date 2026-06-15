using TemplateService.Domain.Primitives;

namespace TemplateService.Domain.Locations;

/// <summary>
/// Локация - место, где работает подразделение.
/// </summary>
public class Location
{
    /// <summary>
    /// Идентификатор локации.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Отображаемое название локации.
    /// </summary>
    public Name Name { get; private set; }

    /// <summary>
    /// Адрес локации.
    /// </summary>
    public Address Address { get; private set; }

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
    private Location(
    Guid id,
    Name name,
    Address address,
    DateTime createdAt,
    DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Address = address;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Создаёт новую локацию.
    /// </summary>
    public static Location Create(
        Guid id,
        string name,
        string address,
        DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("location.id.empty", "Идентификатор локации не может быть пустым.");
        }

        var nameVo = Name.Create(name);
        var addressVo = Address.Create(address);

        return new Location(
        id,
        nameVo,
        addressVo,
        createdAt,
        createdAt);
    }

    /// <summary>
    /// Переименовывает локацию.
    /// </summary>
    public void Rename(string newName, DateTime updatedAt)
    {
        Name = Name.Create(newName);
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Обновляет адрес локации.
    /// </summary>
    public void UpdateAddress(string newAddress, DateTime updatedAt)
    {
        Address = Address.Create(newAddress);
        UpdatedAt = updatedAt;
    }
}
