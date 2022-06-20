using Informator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informator.Database.ModelConfigurations;

internal class MessageConfiguration : IEntityTypeConfiguration<Message> {
    public void Configure(EntityTypeBuilder<Message> builder) {
        builder
            .ToTable(nameof(Message));
        builder
            .HasKey(x => x.Id);
        builder
            .Property(e => e.Text)
            .HasColumnType("text")
            .IsRequired();
        builder
            .Property(e => e.Date)
            .IsRequired();

        builder
            .HasOne(e => e.Sender)
            .WithMany();
    }
}
