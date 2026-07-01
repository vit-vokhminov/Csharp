using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.DepartmentLocations;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="DepartmentLocation"/>.
/// Описывает связующую таблицу между подразделениями и локациями (многие‑ко‑многим),
/// включая составной уникальный индекс, внешние ключи и правила каскадного удаления.
/// </summary>
internal sealed class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    // Имена объектов БД: индексы и внешние ключи.
    // Префиксы: ix_ — индекс (index), fk_ — внешний ключ (foreign key).
    // Предсказуемые имена упрощают отладку, мониторинг и работу с миграциями,
    // в том числе при анализе в pgAdmin 4.
    private const string DepartmentLocationIndex = "ix_department_locations_department_id_location_id";
    private const string LocationIndex = "ix_department_locations_location_id";
    private const string DepartmentFk = "fk_department_locations_department_id";
    private const string LocationFk = "fk_department_locations_location_id";

    /// <summary>
    /// Метод Configure вызывается EF Core при построении модели.
    /// Здесь описывается полное отображение сущности DepartmentLocation на структуру БД.
    /// </summary>
    /// <param name="builder">Построитель конфигурации для типа DepartmentLocation.</param>
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        // Указывает, что сущность DepartmentLocation отображается на таблицу "department_locations"
        // в схеме "directory". Это помогает логически группировать справочные таблицы.
        builder.ToTable("department_locations", "directory");

        // Задаёт первичный ключ сущности — свойство Id.
        // В БД это станет PRIMARY KEY для колонки id.
        builder.HasKey(dl => dl.Id);

        // Настраивает свойство Id:
        // - ValueGeneratedNever() означает, что значение Id НЕ генерируется автоматически
        //   базой данных или EF Core: его должен явно задать код (например, через фабричный метод Create()).
        //   Это согласуется с подходом к агрегатам и инвариантам, который вы разбирали.
        // - HasColumnName("id") явно задаёт имя колонки в БД как "id".
        builder.Property(dl => dl.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        // Настраивает DepartmentId:
        // - Обязательное поле (NOT NULL), колонка "department_id".
        // Это внешний ключ, связывающий запись с подразделением.
        builder.Property(dl => dl.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        // Настраивает LocationId:
        // - Обязательное поле (NOT NULL), колонка "location_id".
        // Это внешний ключ, связывающий запись с локацией.
        builder.Property(dl => dl.LocationId)
            .IsRequired()
            .HasColumnName("location_id");

        // Настраивает IsPrimary:
        // - Флаг, указывающий, является ли данная локация основной для подразделения.
        // - Обязательное поле: в доменной модели это помогает поддерживать инвариант
        //   «у подразделения есть ровно одна основная локация» (логика проверки — на уровне сервиса/агрегата).
        // - Колонка "is_primary" в БД.
        builder.Property(dl => dl.IsPrimary)
            .IsRequired()
            .HasColumnName("is_primary");

        // Настраивает CreatedAt:
        // - Обязательное поле, колонка "created_at".
        // Заполняется при создании записи (например, в фабричном методе или перехватчике),
        // чтобы фиксировать момент привязки локации к подразделению.
        builder.Property(dl => dl.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        // Настраивает UpdatedAt:
        // - Обязательное поле, колонка "updated_at".
        // Обновляется при каждом изменении записи (например, при смене флага IsPrimary),
        // полезно для аудита и отслеживания истории изменений.
        builder.Property(dl => dl.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        // Создаёт составной уникальный индекс по DepartmentId и LocationId:
        // - Гарантирует, что одна и та же пара (подразделение, локация) не может встречаться дважды.
        // - Это реализует ограничение уникальности на уровне БД для связи многие‑ко‑многим.
        // - .HasDatabaseName задаёт понятное имя индекса, что удобно при диагностике ошибок (в т.ч. FATAL/ошибок БД).
        builder.HasIndex(dl => new { dl.DepartmentId, dl.LocationId })
            .IsUnique()
            .HasDatabaseName(DepartmentLocationIndex);

        // Создаёт обычный индекс по LocationId:
        // - Ускоряет выборки вида «все подразделения, привязанные к данной локации».
        // - Полезен для отчётов, списков и API‑эндпоинтов, где поиск идёт по локации.
        builder.HasIndex(dl => dl.LocationId)
            .HasDatabaseName(LocationIndex);

        // Описывает связь с сущностью Department:
        // - HasOne<Domain.Departments.Department>() — каждая запись DepartmentLocation относится к одному подразделению.
        // - WithMany() — у подразделения может быть много связанных локаций.
        // - HasForeignKey(dl => dl.DepartmentId) — связь идёт через колонку department_id.
        // - OnDelete(DeleteBehavior.Cascade) — при удалении подразделения автоматически удаляются все его связи с локациями.
        //   Это упрощает поддержку целостности данных и уменьшает риск «висячих» записей.
        // - HasConstraintName задаёт понятное имя внешнего ключа в БД.
        builder.HasOne<Domain.Departments.Department>()
            .WithMany()
            .HasForeignKey(dl => dl.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(DepartmentFk);

        // Описывает связь с сущностью Location:
        // - HasOne<Domain.Locations.Location>() — каждая запись DepartmentLocation относится к одной локации.
        // - WithMany() — у локации может быть много подразделений.
        // - HasForeignKey(dl => dl.LocationId) — связь идёт через колонку location_id.
        // - OnDelete(DeleteBehavior.Cascade) — при удалении локации автоматически удаляются все её связи с подразделениями.
        // - HasConstraintName задаёт понятное имя внешнего ключа в БД.
        builder.HasOne<Domain.Locations.Location>()
            .WithMany()
            .HasForeignKey(dl => dl.LocationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(LocationFk);
    }
}
