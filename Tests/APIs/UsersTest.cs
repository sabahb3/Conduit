using System.Net;
using System.Net.Http.Headers;
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
    [Fact]
    public async Task ShouldReturn200WhenAskAboutUser()
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
        
        var user = new
        {
            email = "sabahBaara4@gmail.com",
            password = "4050"
        };
        var objAsJson = JsonConvert.SerializeObject(user);
        var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

        var loginUri = new Uri("https://localhost:7240/api/users/login");
        var loginResponse = await client.PostAsync(loginUri, content);
        var token = await loginResponse.Content.ReadAsStringAsync();
        var uri = new Uri("https://localhost:7240/api/user");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync(uri);
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be((HttpStatusCode)200);
        body.Should().Contain("username");
        body.Should().Contain("email");
        body.Should().Contain("bio");
        body.Should().Contain("image");
        body.Should().Contain("token");
    }
}