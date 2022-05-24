using Conduit.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests;

internal class Conduit : WebApplicationFactory<Program>
{
    private readonly string _environment;
    public DbContext Context { get; set; }

    public Conduit(string environment = "Development")
    {
        _environment = environment;
    }
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);
        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp =>
            {
                var options = new DbContextOptionsBuilder<ConduitDbContext>()
                    .UseInMemoryDatabase("Tests")
                    .UseApplicationServiceProvider(sp)
                    .Options;
                Context = new ConduitDbContext(options);
                return options;
            });
        });

        return base.CreateHost(builder);
    }
}