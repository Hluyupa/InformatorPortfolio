using Informator.Database.ModelConfigurations;
using Informator.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Informator.Database;

public class InformatorContext : IdentityDbContext<UserIdentity, UserIdentityRole, Guid> {
    public InformatorContext(DbContextOptions options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new DataUserConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new SystemTypeConfiguration());

    }

    public virtual DbSet<Contact>? Contacts { get; set; }
    public virtual DbSet<Group>? Groups { get; set; }
    public virtual DbSet<Message>? Messages { get; set; }
    public virtual DbSet<SystemType>? SystemTypes { get; set; }
    public virtual DbSet<DataUser>? DataUsers { get; set; }
    
}