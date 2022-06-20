using Informator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informator.Database.ModelConfigurations;

internal class DataUserConfiguration : IEntityTypeConfiguration<DataUser> {
    public void Configure(EntityTypeBuilder<DataUser> builder) {
        builder
            .ToTable(nameof(DataUser));
        builder
            .HasKey(x => x.Id);
        builder
            .Property(e => e.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(e => e.SecondName)
            .HasMaxLength(150)
            .IsRequired();
        builder
            .Property(e => e.MiddleName)
            .HasMaxLength(150)
            .IsRequired(false);
        builder
            .Property(e => e.Email)
            .HasMaxLength(256)
            .IsRequired();
        builder
            .Property(e => e.PhoneNumber)
            .HasMaxLength(25)
            .IsRequired(false);
        builder
            .Property(e => e.PlaceOfWork)
            .HasMaxLength(256)
            .IsRequired(false);

        builder
            .HasOne(e => e.UserIdentity)
            .WithOne(e => e.DataUser)
            .HasForeignKey<UserIdentity>(e => e.Id);

        builder
            .HasMany(e => e.Contacts)
            .WithOne(e => e.User);
        builder
            .HasMany(e => e.Groups)
            .WithMany(e => e.Members);
    }
}
