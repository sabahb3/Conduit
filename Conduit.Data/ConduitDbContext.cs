using Conduit.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Conduit.Data;

public class ConduitDbContext : DbContext
{
    public DbSet<Users> Users { get; set; }
    public DbSet<Articles> Articles { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbContextOptionsBuilder ConduitOptions { get; private set; }


    public ConduitDbContext()
    {
        
    }

    public ConduitDbContext(DbContextOptions options): base(options)
    {
        
    }
    public static readonly ILoggerFactory ConsoleLoggerFactory
        = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
        });

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory).EnableSensitiveDataLogging()
                .UseSqlServer(
                    @"Server=localhost;
                    Database=Conduit;
                    Trusted_Connection=True;")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        ConduitOptions = optionsBuilder;
    }
}