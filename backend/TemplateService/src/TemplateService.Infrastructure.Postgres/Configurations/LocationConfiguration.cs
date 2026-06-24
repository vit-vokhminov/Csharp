using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.Locations;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="Location"/>.
/// </summary>
internal sealed class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    private const string LocationNameIndex = "ix_locations_name";

    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations", "directory");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name")
            .HasConversion(ValueObjectConverters.Name); // Конвертация имени локации

        builder.Property(l => l.Address)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("address")
            .HasConversion(ValueObjectConverters.Address); // Конвертация адреса локации

        builder.Property(l => l.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(l => l.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(l => l.Name)
            .IsUnique()
            .HasDatabaseName(LocationNameIndex);
    }
}
