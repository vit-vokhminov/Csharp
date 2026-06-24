using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.Departments;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="Department"/>.
/// </summary>
internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    private const string DepartmentSlugIndex = "ix_departments_slug";
    private const string DepartmentPathIndex = "ix_departments_path";
    private const string DepartmentParentIndex = "ix_departments_parent_id";
    private const string DepartmentParentFk = "fk_departments_parent_id";

    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments", "directory"); // Исправлено: "departments" → "departments"

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name")
            .HasConversion(ValueObjectConverters.Name); // Конвертация имени подразделения

        builder.Property(d => d.Slug)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("slug")
            .HasConversion(ValueObjectConverters.Slug); // Конвертация slug подразделения

        builder.Property(d => d.Path)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("path")
            .HasConversion(ValueObjectConverters.TreePath); // Конвертация иерархического пути

        builder.Property(d => d.ParentId)
            .IsRequired(false) // Явное указание, что может быть null для корневых подразделений
            .HasColumnName("parent_id");

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(d => d.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(d => d.Slug)
            .IsUnique()
            .HasDatabaseName(DepartmentSlugIndex);

        builder.HasIndex(d => d.Path)
            .IsUnique()
            .HasDatabaseName(DepartmentPathIndex);

        builder.HasIndex(d => d.ParentId)
            .HasDatabaseName(DepartmentParentIndex);

        // Иерархическая связь с самим собой: подразделение может иметь родителя
        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName(DepartmentParentFk);
    }
}
