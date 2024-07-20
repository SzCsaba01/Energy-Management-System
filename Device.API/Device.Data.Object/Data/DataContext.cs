using Microsoft.EntityFrameworkCore;
using Project.Data.Data.Entities;

namespace Project.Data.Data;
public class DataContext : DbContext {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<DeviceEntity> Devices { get; set; }
}