
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TemplateService.Domain.Primitives;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Единое место для value‑конвертеров доменных примитивов.
/// Позволяет хранить Value Objects как колонки в таблицах их владельцев.
/// </summary>
internal static class ValueObjectConverters
{
    /// <summary>
    /// Конвертер для <see cref="Name"/>.
    /// </summary>
    public static readonly ValueConverter<Name, string> Name = new(
        name => name.Value,
        value => Domain.Primitives.Name.Create(value));

    /// <summary>
    /// Конвертер для <see cref="Slug"/>.
    /// </summary>
    public static readonly ValueConverter<Slug, string> Slug = new(
        slug => slug.Value,
        value => Domain.Primitives.Slug.Create(value));

    /// <summary>
    /// Конвертер для <see cref="TreePath"/>.
    /// </summary>
    public static readonly ValueConverter<TreePath, string> TreePath = new(
            path => path.Value,
            value => Domain.Primitives.TreePath.Create(value));

    /// <summary>
    /// Конвертер для <see cref="Address"/>.
    /// </summary>
    public static readonly ValueConverter<Address, string> Address = new(
        address => address.Value,
        value => Domain.Primitives.Address.Create(value));
}
