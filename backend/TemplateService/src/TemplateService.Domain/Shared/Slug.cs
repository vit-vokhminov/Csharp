using System.Text.RegularExpressions;

namespace TemplateService.Domain.Primitives;

/// <summary>
/// Короткий стабильный код сущности для URL, интеграций и дерева.
/// Не равен отображаемому названию: name можно переименовать, а стабильный код лучше не менять.
/// </summary>
public sealed class Slug : ValueObject
{
    /// <summary>
    /// Регулярное выражение для валидации slug: только строчные латинские буквы, цифры и дефисы
    /// </summary>
    private static readonly Regex SlugPattern = new(
       @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
       RegexOptions.Compiled,
       TimeSpan.FromMilliseconds(100));

    public string Value { get; }

    private Slug(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Создаёт Slug из строки. Выбрасывает DomainException при невалидном значении.
    /// </summary>
    public static Slug Create(string value)
    {
        if (value is null)
        {
            throw new DomainException("slug.null", "Slug не может быть null.");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("slug.empty", "Slug не может быть пустым.");
        }

        if (value.Length > 100)
        {
            throw new DomainException("slug.too.long", "Slug не может быть длиннее 100 символов.");
        }

        if (!SlugPattern.IsMatch(value))
        {
            throw new DomainException(
                "slug.invalid.format",
                "Slug может содержать только строчные латинские буквы, цифры и дефисы. Пример: 'sales', 'b2b-sales'.");
        }

        return new Slug(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
