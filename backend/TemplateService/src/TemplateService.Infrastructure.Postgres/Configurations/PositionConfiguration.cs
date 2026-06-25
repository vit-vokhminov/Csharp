using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateService.Domain.Positions;

namespace TemplateService.Infrastructure.Postgres.Configurations;

/// <summary>
/// Конфигурация EF Core для сущности <see cref="Position"/>.
/// </summary>
internal sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    private const string PositionNameIndex = "ix_positions_name";

    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions", "directory");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name")
            .HasConversion(ValueObjectConverters.Name); // Конвертация имени позиции

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName(PositionNameIndex);
    }
}
