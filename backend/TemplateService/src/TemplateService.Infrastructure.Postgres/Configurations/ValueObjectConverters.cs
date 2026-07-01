using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TemplateService.Domain.Primitives;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Единое место для value‑конвертеров доменных примитивов.
/// Позволяет хранить Value Objects как колонки в таблицах их владельцев.
/// 
/// Зачем это нужно:
/// - В БД хранятся простые типы (в данном случае — string), что упрощает схему и индексацию.
/// - В доменной модели работают полноценные ValueObject с инвариантами (валидацией при создании).
/// - Конвертация происходит автоматически при чтении/записи через EF Core.
/// Это напрямую связано с темой агрегатов и инвариантов.
/// </summary>
internal static class ValueObjectConverters
{
    /// <summary>
    /// Конвертер для <see cref="Name"/>.
    /// Преобразует ValueObject Name ↔ строку для хранения в БД.
    /// </summary>
    public static readonly ValueConverter<Name, string> Name = new(
        // Функция «из домена в БД»: берёт внутреннее значение ValueObject (строку).
        name => name.Value,
        // Функция «из БД в домен»: создаёт ValueObject через фабричный метод Create(value).
        // Это критически важно: если value невалиден, Create выбросит ошибку или вернёт валидный объект —
        // так сохраняются инварианты ValueObject.
        value => Domain.Primitives.Name.Create(value));

    /// <summary>
    /// Конвертер для <see cref="Slug"/>.
    /// Slug — обычно короткий URL‑идентификатор (например, «my-page»).
    /// Хранится в БД как строка, в коде — как ValueObject со своей валидацией.
    /// </summary>
    public static readonly ValueConverter<Slug, string> Slug = new(
        slug => slug.Value,
        value => Domain.Primitives.Slug.Create(value));

    /// <summary>
    /// Конвертер для <see cref="TreePath"/>.
    /// TreePath — путь в дереве (например, для категорий/иерархий).
    /// В БД хранится как строка (часто в формате ltree или просто «1.2.3»),
    /// в домене — как ValueObject, который может предоставлять методы навигации по дереву.
    /// </summary>
    public static readonly ValueConverter<TreePath, string> TreePath = new(
        path => path.Value,
        value => Domain.Primitives.TreePath.Create(value));

    /// <summary>
    /// Конвертер для <see cref="Address"/>.
    /// Адрес — типичный ValueObject: имеет инварианты (обязательные части, формат),
    /// но в БД удобно хранить как одну строку (или JSON — здесь выбрано хранение как string).
    /// Валидация происходит на этапе создания через Address.Create(value).
    /// </summary>
    public static readonly ValueConverter<Address, string> Address = new(
        address => address.Value,
        value => Domain.Primitives.Address.Create(value));
}
