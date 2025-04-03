using Microsoft.EntityFrameworkCore;

namespace FinBeatTech.Database;

public class FinBeatDbContext : DbContext
{
    public FinBeatDbContext(DbContextOptions<FinBeatDbContext> options) : base(options)
    {
    }

    public DbSet<Data> DataEntries { get; set; }
    public DbSet<ApiLog> ApiLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Data>().HasKey(k => k.Id);
        modelBuilder.Entity<ApiLog>().HasKey(k => k.Id);

        modelBuilder.Entity<Data>().Property(p => p.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<ApiLog>().Property(p => p.Id).ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Data>().Property(p => p.Code).IsRequired();
        modelBuilder.Entity<Data>().Property(p => p.Value).IsRequired();
        
        modelBuilder.Entity<ApiLog>().Property(p => p.Method).IsRequired();
        modelBuilder.Entity<ApiLog>().Property(p => p.Path).IsRequired();
        modelBuilder.Entity<ApiLog>().Property(p => p.StatusCode).IsRequired();
        modelBuilder.Entity<ApiLog>().Property(p => p.Timestamp).IsRequired();
    }
}