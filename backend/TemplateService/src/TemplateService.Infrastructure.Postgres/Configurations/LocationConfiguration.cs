using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.Locations;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="Location"/>.
/// Определяет, как сущность Location отображается на таблицу в PostgreSQL:
/// задаёт имя таблицы и схемы, первичные ключи, свойства, их ограничения и индексы.
/// </summary>
internal sealed class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    // Имя индекса для поля Name в базе данных.
    // Префикс ix_ — общепринятое соглашение для индексов (index).
    private const string LocationNameIndex = "ix_locations_name";

    /// <summary>
    /// Метод Configure вызывается EF Core при построении модели.
    /// Здесь описывается маппинг сущности Location на таблицу БД.
    /// </summary>
    /// <param name="builder">Построитель конфигурации для типа Location.</param>
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        // Указывает, что сущность Location должна отображаться на таблицу "locations"
        // в схеме "directory" (а не в схеме по умолчанию public).
        builder.ToTable("locations", "directory");

        // Задаёт первичный ключ сущности — свойство Id.
        // В БД это станет PRIMARY KEY для колонки id.
        builder.HasKey(l => l.Id);

        // Настраивает свойство Id:
        // - ValueGeneratedNever() означает, что значение Id НЕ генерируется автоматически
        //   базой данных или EF Core: его должен явно задать код (например, через фабричный метод).
        // - HasColumnName("id") явно задаёт имя колонки в БД как "id".
        builder.Property(l => l.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        // Настраивает свойство Name:
        // - IsRequired() делает поле обязательным (NOT NULL в БД).
        // - HasMaxLength(200) ограничивает длину строки до 200 символов.
        // - HasColumnName("name") задаёт имя колонки как "name".
        // - HasConversion(ValueObjectConverters.Name) применяет конвертер:
        //   позволяет хранить в БД примитив (например, строку), а в коде работать с ValueObject.
        //   Это поддерживает инварианты ValueObject.
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name")
            .HasConversion(ValueObjectConverters.Name); // Конвертация имени локации

        // Настраивает свойство Address аналогично Name:
        // - NOT NULL, до 500 символов, колонка "address".
        // - Конвертер ValueObjectConverters.Address обеспечивает валидацию и инварианты адреса
        //   на уровне ValueObject, а в БД сохраняется его внутреннее представление.
        builder.Property(l => l.Address)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("address")
            .HasConversion(ValueObjectConverters.Address); // Конвертация адреса локации

        // Настраивает CreatedAt:
        // - Обязательное поле, колонка "created_at".
        // Обычно заполняется в коде (например, в фабричном методе или перехватчике SaveChanges).
        builder.Property(l => l.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        // Настраивает UpdatedAt:
        // - Обязательное поле, колонка "updated_at".
        // Обновляется при каждом изменении записи (обычно через перехватчики или триггеры).
        builder.Property(l => l.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        // Создаёт уникальный индекс по полю Name:
        // - Гарантирует, что в таблице не будет двух локаций с одинаковым именем.
        // - .HasDatabaseName(LocationNameIndex) задаёт конкретное имя индекса в БД,
        //   чтобы оно было предсказуемым (удобно для миграций, мониторинга и отладки).
        builder.HasIndex(l => l.Name)
            .IsUnique()
            .HasDatabaseName(LocationNameIndex);
    }
}
