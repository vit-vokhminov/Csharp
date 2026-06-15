namespace TemplateService.Domain.Primitives;

/// <summary>
/// Отображаемое название сущности. Может быть переименовано без изменения стабильного slug.
/// </summary>
public sealed class Name : ValueObject
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Создаёт Name из строки. Выбрасывает DomainException при невалидном значении.
    /// </summary>
    public static Name Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("name.empty", "Название не может быть пустым.");
        }

        var trimmed = value.Trim();

        // Проверка, что после обрезки строка не стала пустой
        if (string.IsNullOrEmpty(trimmed))
        {
            throw new DomainException("name.empty.after.trim", "Название не может состоять только из пробелов.");
        }

        if (trimmed.Length > 200)
        {
            throw new DomainException("name.too.long", "Название не может быть длиннее 200 символов.");
        }

        return new Name(trimmed);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
