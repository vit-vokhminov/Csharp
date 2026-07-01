using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.Departments;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="Department"/>.
/// Описывает маппинг подразделения на таблицу departments в схеме directory,
/// включая иерархию (родительское подразделение), уникальные ограничения и индексы.
/// </summary>
internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    // Имена объектов БД: индексы и внешний ключ.
    // Префиксы: ix_ — индекс (index), fk_ — внешний ключ (foreign key).
    // Это даёт предсказуемые имена в миграциях и упрощает отладку (в т.ч. при работе с pgAdmin 4, о которой вы спрашивали).
    private const string DepartmentSlugIndex = "ix_departments_slug";
    private const string DepartmentPathIndex = "ix_departments_path";
    private const string DepartmentParentIndex = "ix_departments_parent_id";
    private const string DepartmentParentFk = "fk_departments_parent_id";

    /// <summary>
    /// Метод Configure вызывается EF Core при построении модели.
    /// Здесь описывается полное отображение сущности Department на структуру БД.
    /// </summary>
    /// <param name="builder">Построитель конфигурации для типа Department.</param>
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Указывает, что сущность Department отображается на таблицу "departments"
        // в схеме "directory". Это помогает логически группировать справочники.
        builder.ToTable("departments", "directory");

        // Задаёт первичный ключ сущности — свойство Id.
        // В БД это станет PRIMARY KEY для колонки id.
        builder.HasKey(d => d.Id);

        // Настраивает свойство Id:
        // - ValueGeneratedNever() означает, что значение Id НЕ генерируется автоматически
        //   базой данных или EF Core: его должен явно задать код (например, через фабричный метод Create()).
        //   Это согласуется с подходом к агрегатам и инвариантам, который вы разбирали.
        // - HasColumnName("id") явно задаёт имя колонки в БД как "id".
        builder.Property(d => d.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        // Настраивает свойство Name:
        // - IsRequired() делает поле обязательным (NOT NULL).
        // - HasMaxLength(200) ограничивает длину строки до 200 символов.
        // - HasColumnName("name") задаёт имя колонки как "name".
        // - HasConversion(ValueObjectConverters.Name) применяет конвертер:
        //   в коде работает ValueObject Name с валидацией, в БД хранится строка.
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name")
            .HasConversion(ValueObjectConverters.Name); // Конвертация имени подразделения

        // Настраивает свойство Slug:
        // - Slug — человекопонятный идентификатор (часто для URL).
        // - Ограничение длины 100 символов, колонка "slug".
        // - Конвертер ValueObjectConverters.Slug обеспечивает валидацию формата slug
        //   при создании ValueObject (через фабричный метод), сохраняя инварианты.
        builder.Property(d => d.Slug)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("slug")
            .HasConversion(ValueObjectConverters.Slug); // Конвертация slug подразделения

        // Настраивает свойство Path:
        // - Path описывает положение подразделения в иерархии (например, "1.2.3").
        // - Длина до 500 символов достаточна для глубоких деревьев.
        // - Конвертер ValueObjectConverters.TreePath позволяет работать с деревом
        //   на уровне домена, а в БД хранить компактную строку.
        builder.Property(d => d.Path)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("path")
            .HasConversion(ValueObjectConverters.TreePath); // Конвертация иерархического пути

        // Настраивает ParentId — ссылку на родительское подразделение:
        // - IsRequired(false) явно разрешает значение null: корневые подразделения не имеют родителя.
        // - HasColumnName("parent_id") задаёт имя колонки.
        // Это реализует самоссылающуюся (рекурсивную) связь «подразделение → родитель».
        builder.Property(d => d.ParentId)
            .IsRequired(false) // Явное указание, что может быть null для корневых подразделений
            .HasColumnName("parent_id");

        // Настраивает CreatedAt:
        // - Обязательное поле, колонка "created_at".
        // Заполняется при создании агрегата (например, в фабричном методе), чтобы фиксировать момент создания.
        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        // Настраивает UpdatedAt:
        // - Обязательное поле, колонка "updated_at".
        // Обновляется при каждом изменении записи, полезно для аудита и версионирования.
        builder.Property(d => d.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        // Создаёт уникальный индекс по Slug:
        // - Гарантирует, что у двух подразделений не будет одинакового slug.
        // - .HasDatabaseName задаёт конкретное имя индекса в БД (удобно для миграций и мониторинга).
        builder.HasIndex(d => d.Slug)
            .IsUnique()
            .HasDatabaseName(DepartmentSlugIndex);

        // Создаёт уникальный индекс по Path:
        // - В древовидной структуре путь должен быть уникальным: каждое подразделение
        //   занимает одну позицию в дереве.
        // - Это ускоряет поиск по пути и защищает от дублирования позиций в иерархии.
        builder.HasIndex(d => d.Path)
            .IsUnique()
            .HasDatabaseName(DepartmentPathIndex);

        // Создаёт обычный индекс по ParentId:
        // - Ускоряет выборки «все дочерние подразделения для данного родителя».
        // - Важен для производительности при обходе дерева и отображения списков.
        builder.HasIndex(d => d.ParentId)
            .HasDatabaseName(DepartmentParentIndex);

        // Описывает иерархическую связь «само на себя»:
        // - HasOne<Department>() — каждое подразделение имеет не более одного родителя.
        // - WithMany() — у родителя может быть много дочерних подразделений.
        // - HasForeignKey(d => d.ParentId) — связь идёт через колонку parent_id.
        // - OnDelete(DeleteBehavior.Restrict) — запрещает удаление родителя, если у него есть дети.
        //   Это защищает целостность иерархии и согласуется с идеей инвариантов агрегата.
        // - HasConstraintName задаёт понятное имя внешнего ключа в БД.
        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(DepartmentParentFk);
    }
}
