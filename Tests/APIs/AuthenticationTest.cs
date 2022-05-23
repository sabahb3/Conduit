using System.Net;
using System.Text;
using AutoMapper;
using Conduit.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tests.APIs;

public class AuthenticationTest
{
    [Fact]
    public async Task ShouldReturn200WhenLogin()
    {
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var user = new
        {
            email = "sabahBaara4@gmail.com",
            password = "4050"
        };
        var objAsJson = JsonConvert.SerializeObject(user);
        var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

        var uri = new Uri("https://localhost:7240/api/users/login");
        var response = await client.PostAsync(uri, content);

        response.StatusCode.Should().Be((HttpStatusCode)200);
        
    }
    [Fact]
    public async Task ShouldReturn404WhenLogin()
    {
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();
        var user = new
        {
            email = "sabahBaara@gmail.com",
            password = "4050"
        };
        var objAsJson = JsonConvert.SerializeObject(user);
        var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

        var uri = new Uri("https://localhost:7240/api/users/login");
        var response = await client.PostAsync(uri, content);

        response.StatusCode.Should().Be((HttpStatusCode)404);
        
    }
}