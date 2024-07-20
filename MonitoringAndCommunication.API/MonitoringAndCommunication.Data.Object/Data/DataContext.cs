using Microsoft.EntityFrameworkCore;
using MonitoringAndCommunication.Data.Object.Entities;

namespace MonitoringAndCommunication.Data.Object.Data;
public class DataContext : DbContext {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<DeviceEntity>()
            .HasIndex(u => u.Id)
            .IsUnique();
        
        builder.Entity<DeviceEntity>()
            .HasMany(x => x.Monitorings)
            .WithOne(x => x.Device)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<DeviceEntity> Devices { get; set; }
    public DbSet<MonitoringEntity> Monitorings { get; set; }
}
