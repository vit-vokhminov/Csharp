namespace TemplateService.Domain.Primitives;

/// <summary>
/// Адрес локации.
/// </summary>
public sealed class Address : ValueObject
{
    public string Value { get; }

    private Address(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Создаёт Address из строки. Выбрасывает DomainException при невалидном значении.
    /// </summary>
    /// <param name="value">Строковое представление адреса.</param>
    /// <returns>Экземпляр Address.</returns>
    /// <exception cref="DomainException">Выбрасывается, если адрес пуст или слишком длинный.</exception>
    public static Address Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("address.empty", "Адрес не может быть пустым.");
        }

        var trimmed = value.Trim();

        // Проверка, что после обрезки строка не стала пустой
        if (string.IsNullOrEmpty(trimmed))
        {
            throw new DomainException("address.empty.after.trim", "Адрес не может состоять только из пробелов.");
        }

        if (trimmed.Length > 500)
        {
            throw new DomainException("address.too.long", "Адрес не может быть длиннее 500 символов.");
        }

        return new Address(trimmed);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
