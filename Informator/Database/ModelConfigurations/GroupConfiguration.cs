using Informator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informator.Database.ModelConfigurations;

internal class GroupConfiguration : IEntityTypeConfiguration<Group> {
    public void Configure(EntityTypeBuilder<Group> builder) {
        builder
            .ToTable(nameof(Group));
        builder
            .HasKey(x => x.Id);
        builder
            .Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
