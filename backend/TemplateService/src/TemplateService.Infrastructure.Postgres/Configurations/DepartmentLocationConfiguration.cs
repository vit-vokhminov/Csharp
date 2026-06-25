using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.DepartmentLocations;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="DepartmentLocation"/>.
/// </summary>
internal sealed class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    private const string DepartmentLocationIndex = "ix_department_locations_department_id_location_id";
    private const string LocationIndex = "ix_department_locations_location_id";
    private const string DepartmentFk = "fk_department_locations_department_id";
    private const string LocationFk = "fk_department_locations_location_id";

    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_locations", "directory");

        builder.HasKey(dl => dl.Id);

        builder.Property(dl => dl.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(dl => dl.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        builder.Property(dl => dl.LocationId)
            .IsRequired()
            .HasColumnName("location_id");

        builder.Property(dl => dl.IsPrimary)
            .IsRequired()
            .HasColumnName("is_primary");

        builder.Property(dl => dl.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(dl => dl.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(dl => new { dl.DepartmentId, dl.LocationId })
            .IsUnique()
            .HasDatabaseName(DepartmentLocationIndex);

        builder.HasIndex(dl => dl.LocationId)
            .HasDatabaseName(LocationIndex);

        builder.HasOne<Domain.Departments.Department>()
            .WithMany()
            .HasForeignKey(dl => dl.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(DepartmentFk);

        builder.HasOne<Domain.Locations.Location>()
            .WithMany()
            .HasForeignKey(dl => dl.LocationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(LocationFk);
    }
}
