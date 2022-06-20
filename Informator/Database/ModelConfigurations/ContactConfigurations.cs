using Informator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informator.Database.ModelConfigurations;

internal class ContactConfiguration : IEntityTypeConfiguration<Contact> {
    public void Configure(EntityTypeBuilder<Contact> builder) {
        builder
            .ToTable(nameof(Contact));
        builder
            .Property(e => e.Data)
            .HasColumnType("nvarchar(max)");
        builder
            .HasKey(x => x.Id);
        builder
            .HasOne(e => e.SystemType)
            .WithMany();
    }
}
