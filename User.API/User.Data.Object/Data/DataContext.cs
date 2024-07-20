using Microsoft.EntityFrameworkCore;
using Project.Data.Data.Entities;
using User.Data.Object.Entities;

namespace Project.Data.Data;
public class DataContext : DbContext {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();
        builder.Entity<UserEntity>()
            .HasIndex(u => u.Username)
            .IsUnique();

        builder.Entity<UserEntity>()
            .HasMany(u => u.Devices)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<DeviceEntity> Devices { get; set; }
}