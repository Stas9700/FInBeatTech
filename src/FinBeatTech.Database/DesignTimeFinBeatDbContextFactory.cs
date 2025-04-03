using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinBeatTech.Database;

public class DesignTimeFinBeatDbContextFactory: IDesignTimeDbContextFactory<FinBeatDbContext>
{
    public FinBeatDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<FinBeatDbContext> options = new DbContextOptionsBuilder<FinBeatDbContext>();
        const string dbName = "fintech.db";
        var defaultPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()
            ?.Location);
        Console.WriteLine("Default:" + defaultPath);
        var fullPath =
            Path.Combine(defaultPath ?? throw new InvalidOperationException(nameof(defaultPath)), dbName);
        Console.WriteLine("Full:" + fullPath);
        options.UseSqlite($"Data Source={fullPath}");
        return new FinBeatDbContext(options.Options);
    }
}