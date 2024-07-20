using Chat.Data.Objects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data.Objects.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<MessageEntity> Messages { get; set; }
}