using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.DepartmentPositions;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="DepartmentPosition"/>.
/// </summary>
internal sealed class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    private const string DepartmentPositionIndex = "ix_department_positions_department_id_position_id";
    private const string PositionIndex = "ix_department_positions_position_id";
    private const string DepartmentFk = "fk_department_positions_department_id";
    private const string PositionFk = "fk_department_positions_position_id";

    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_positions", "directory");

        builder.HasKey(dp => dp.Id);

        builder.Property(dp => dp.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(dp => dp.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        builder.Property(dp => dp.PositionId)
            .IsRequired()
            .HasColumnName("position_id");

        builder.Property(dp => dp.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(dp => dp.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(dp => new { dp.DepartmentId, dp.PositionId })
            .IsUnique()
            .HasDatabaseName(DepartmentPositionIndex);

        builder.HasIndex(dp => dp.PositionId)
            .HasDatabaseName(PositionIndex);

        builder.HasOne<Domain.Departments.Department>()
            .WithMany()
            .HasForeignKey(dp => dp.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(DepartmentFk);

        builder.HasOne<Domain.Positions.Position>()
            .WithMany()
            .HasForeignKey(dp => dp.PositionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(PositionFk);
    }
}
