using Informator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informator.Database.ModelConfigurations;

internal class SystemTypeConfiguration : IEntityTypeConfiguration<SystemType> {
    public void Configure(EntityTypeBuilder<SystemType> builder) {
        builder
            .ToTable(nameof(SystemType));
        builder
            .HasKey(x => x.Id);
        builder
            .Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(e => e.ModelEntity)
            .HasMaxLength(255)
            .IsRequired();
    }
}