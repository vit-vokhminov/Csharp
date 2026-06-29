using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.Positions;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="Position"/>.
/// Определяет маппинг сущности Position на таблицу в PostgreSQL:
/// задаёт имя таблицы и схемы, первичный ключ, свойства, их ограничения и индексы.
/// </summary>
internal sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    // Имя индекса для поля Name в базе данных.
    // Префикс ix_ — общепринятое соглашение для именования индексов (index).
    private const string PositionNameIndex = "ix_positions_name";

    /// <summary>
    /// Метод Configure вызывается EF Core при построении модели.
    /// Здесь описывается, как сущность Position отображается на структуру БД.
    /// </summary>
    /// <param name="builder">Построитель конфигурации для типа Position.</param>
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        // Указывает, что сущность Position должна отображаться на таблицу "positions"
        // в схеме "directory" (не в схеме public по умолчанию).
        // Это помогает логически разделять таблицы (например, справочники в directory).
        builder.ToTable("positions", "directory");

        // Задаёт первичный ключ сущности — свойство Id.
        // В БД это станет PRIMARY KEY для колонки id.
        builder.HasKey(p => p.Id);

        // Настраивает свойство Id:
        // - ValueGeneratedNever() означает, что значение Id НЕ генерируется автоматически
        //   базой данных или EF Core: его должен явно задать код (например, через статический фабричный метод Create(),
        //   о котором вы спрашивали ранее).
        // - HasColumnName("id") явно задаёт имя колонки в БД как "id".
        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        // Настраивает свойство Name:
        // - IsRequired() делает поле обязательным (NOT NULL в БД).
        // - HasMaxLength(200) ограничивает длину строки до 200 символов.
        // - HasColumnName("name") задаёт имя колонки как "name".
        // - HasConversion(ValueObjectConverters.Name) применяет конвертер:
        //   позволяет хранить в БД примитив (например, строку), а в коде работать с ValueObject.
        //   Это поддерживает инварианты ValueObject (о чём вы ранее спрашивали).
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name")
            .HasConversion(ValueObjectConverters.Name); // Конвертация имени позиции

        // Настраивает CreatedAt:
        // - Обязательное поле, колонка "created_at".
        // Обычно заполняется в коде (например, в фабричном методе или перехватчике SaveChanges),
        // чтобы гарантировать соблюдение инвариантов агрегата.
        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        // Настраивает UpdatedAt:
        // - Обязательное поле, колонка "updated_at".
        // Обновляется при каждом изменении записи (обычно через перехватчики или триггеры).
        // Помогает отслеживать время последнего изменения позиции.
        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        // Создаёт уникальный индекс по полю Name:
        // - Гарантирует, что в таблице не будет двух позиций с одинаковым именем.
        // - .HasDatabaseName(PositionNameIndex) задаёт конкретное имя индекса в БД,
        //   чтобы оно было предсказуемым (удобно для миграций, мониторинга и отладки).
        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName(PositionNameIndex);
    }
}
