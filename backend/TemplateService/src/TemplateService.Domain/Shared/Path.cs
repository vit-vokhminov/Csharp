namespace TemplateService.Domain.Primitives;

/// <summary>
/// Полный путь подразделения в дереве, собранный из slug родителей и самого подразделения.
/// Нужен для быстрого поиска ветки оргструктуры, построения breadcrumbs и показа дерева в UI.
/// </summary>
public sealed class TreePath : ValueObject
{
    public string Value { get; }

    private TreePath(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Создаёт TreePath из строки. Выбрасывает DomainException при невалидном значении.
    /// </summary>
    public static TreePath Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("path.empty", "Путь не может быть пустым.");
        }

        if (value.Length > 500)
        {
            throw new DomainException("path.too.long", "Путь не может быть длиннее 500 символов.");
        }

        if (value.StartsWith('/'))
        {
            throw new DomainException("path.leading.slash", "Путь не должен начинаться с '/'.");
        }

        if (value.EndsWith('/'))
        {
            throw new DomainException("path.trailing.slash", "Путь не должен заканчиваться на '/'•");
        }

        var segments = value.Split('/');
        foreach (var segment in segments)
        {
            if (string.IsNullOrWhiteSpace(segment))
            {
                throw new DomainException("path.empty.segment", "Путь содержит пустой сегмент.");
            }

            // Каждый сегмент должен быть валидным slug 
            try
            {
                _ = Slug.Create(segment);
            }
            catch (DomainException)
            {
                throw new DomainException(
                "path.invalid.segment",
                $"Сегмент пути 'segment * не является валидным slug.");
            }
        }

        return new TreePath(value);
    }

    /// <summary>
    /// Создаёт путь для корневого подразделения из его slug.
    /// </summary>
    public static TreePath CreateRoot(Slug slug)
    {
        return new TreePath(slug.Value);
    }

    /// <summary>
    /// Создаёт путь для дочернего подразделения, добавляя slug к пути родителя.
    /// </summary>
    public static TreePath CreateChild(TreePath parentPath, Slug slug)
    {
        return new TreePath($"{parentPath.Value}/{slug.Value}");
    }

    /// <summary>
    /// Возвращает сегменты пути.
    /// </summary>
    public IReadOnlyList<string> Segments => Value.Split('/');

    /// <summary>
    /// Возвращает глубину подразделения в дереве (количество сегментов).
    /// </summary>
    public int Depth => Segments.Count;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
