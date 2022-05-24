using System.Net;
using System.Text;
using Conduit.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Tests.APIs;

public class UsersTest
{
    [Fact]
    public async Task ShouldReturn401WhenAskAboutUser()
    {
        await using var application = new Conduit();
        using var client = application.CreateClient();
        var serviceScopeFactory = application.Services.GetService<IServiceScopeFactory>();
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var option=(DbContextOptions)scope.ServiceProvider.GetService(typeof(DbContextOptions))!;
            using (var context = new ConduitDbContext(option))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        
        var uri = new Uri("https://localhost:7240/api/user");
        var response = await client.GetAsync(uri);

        response.StatusCode.Should().Be((HttpStatusCode)401);   
    }
}