using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.DepartmentPositions;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="DepartmentPosition"/>.
/// Описывает связующую таблицу между подразделениями и должностями (многие‑ко‑многим),
/// включая составной уникальный индекс, внешние ключи и правила каскадного удаления.
/// Такая структура позволяет гибко назначать должности подразделениям, сохраняя целостность данных.
/// </summary>
internal sealed class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    // Имена объектов БД: индексы и внешние ключи.
    // Префиксы: ix_ — индекс (index), fk_ — внешний ключ (foreign key).
    // Предсказуемые имена упрощают отладку, мониторинг и работу с миграциями,
    // в том числе при анализе в pgAdmin 4.
    private const string DepartmentPositionIndex = "ix_department_positions_department_id_position_id";
    private const string PositionIndex = "ix_department_positions_position_id";
    private const string DepartmentFk = "fk_department_positions_department_id";
    private const string PositionFk = "fk_department_positions_position_id";

    /// <summary>
    /// Метод Configure вызывается EF Core при построении модели.
    /// Здесь описывается полное отображение сущности DepartmentPosition на структуру БД.
    /// </summary>
    /// <param name="builder">Построитель конфигурации для типа DepartmentPosition.</param>
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        // Указывает, что сущность DepartmentPosition отображается на таблицу "department_positions"
        // в схеме "directory". Это помогает логически группировать справочные таблицы.
        builder.ToTable("department_positions", "directory");

        // Задаёт первичный ключ сущности — свойство Id.
        // В БД это станет PRIMARY KEY для колонки id.
        builder.HasKey(dp => dp.Id);

        // Настраивает свойство Id:
        // - ValueGeneratedNever() означает, что значение Id НЕ генерируется автоматически
        //   базой данных или EF Core: его должен явно задать код (например, через фабричный метод Create()).
        //   Это согласуется с подходом к агрегатам и инвариантам, который вы разбирали.
        // - HasColumnName("id") явно задаёт имя колонки в БД как "id".
        builder.Property(dp => dp.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        // Настраивает DepartmentId:
        // - Обязательное поле (NOT NULL), колонка "department_id".
        // Это внешний ключ, связывающий запись с подразделением.
        builder.Property(dp => dp.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        // Настраивает PositionId:
        // - Обязательное поле (NOT NULL), колонка "position_id".
        // Это внешний ключ, связывающий запись с должностью.
        builder.Property(dp => dp.PositionId)
            .IsRequired()
            .HasColumnName("position_id");

        // Настраивает CreatedAt:
        // - Обязательное поле, колонка "created_at".
        // Заполняется при создании записи (например, в фабричном методе или перехватчике),
        // чтобы фиксировать момент назначения должности подразделению.
        builder.Property(dp => dp.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        // Настраивает UpdatedAt:
        // - Обязательное поле, колонка "updated_at".
        // Обновляется при каждом изменении записи, полезно для аудита и отслеживания истории изменений.
        builder.Property(dp => dp.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        // Создаёт составной уникальный индекс по DepartmentId и PositionId:
        // - Гарантирует, что одна и та же пара (подразделение, должность) не может встречаться дважды.
        // - Это реализует ограничение уникальности на уровне БД для связи многие‑ко‑многим.
        // - .HasDatabaseName задаёт понятное имя индекса, что удобно при диагностике ошибок.
        builder.HasIndex(dp => new { dp.DepartmentId, dp.PositionId })
            .IsUnique()
            .HasDatabaseName(DepartmentPositionIndex);

        // Создаёт обычный индекс по PositionId:
        // - Ускоряет выборки вида «все подразделения, где назначена данная должность».
        // - Полезен для отчётов, списков и API‑эндпоинтов, где поиск идёт по должности.
        builder.HasIndex(dp => dp.PositionId)
            .HasDatabaseName(PositionIndex);

        // Описывает связь с сущностью Department:
        // - HasOne<Domain.Departments.Department>() — каждая запись DepartmentPosition относится к одному подразделению.
        // - WithMany() — у подразделения может быть много связанных должностей.
        // - HasForeignKey(dp => dp.DepartmentId) — связь идёт через колонку department_id.
        // - OnDelete(DeleteBehavior.Cascade) — при удалении подразделения автоматически удаляются все его связи с должностями.
        //   Это упрощает поддержку целостности данных и уменьшает риск «висячих» записей.
        // - HasConstraintName задаёт понятное имя внешнего ключа в БД.
        builder.HasOne<Domain.Departments.Department>()
            .WithMany()
            .HasForeignKey(dp => dp.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(DepartmentFk);

        // Описывает связь с сущностью Position:
        // - HasOne<Domain.Positions.Position>() — каждая запись DepartmentPosition относится к одной должности.
        // - WithMany() — у должности может быть много подразделений.
        // - HasForeignKey(dp => dp.PositionId) — связь идёт через колонку position_id.
        // - OnDelete(DeleteBehavior.Cascade) — при удалении должности автоматически удаляются все её связи с подразделениями.
        // - HasConstraintName задаёт понятное имя внешнего ключа в БД.
        builder.HasOne<Domain.Positions.Position>()
            .WithMany()
            .HasForeignKey(dp => dp.PositionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(PositionFk);
    }
}
