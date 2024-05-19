
using CleanArchitecture.API.IntegrationTest.Fixtures;
using CleanArchitecture.API.IntegrationTest.Utills;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace CleanArchitecture.API.IntegrationTest.Controllers;

[Collection("API Shared Collection")]
public class StreamerControllerTest : IAsyncLifetime
{
    private readonly WebFactoryTest _factory;
    private readonly HttpClient _httpClient;
    public StreamerControllerTest(WebFactoryTest factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }
    [Fact]
    public async Task GetAllStreamer_ShouldReturn3Streamer()
    {
        // Arrange

        // Action

        var response = await _httpClient.GetFromJsonAsync<List<Streamer>>("api/v1/streamer");

        // Assert
        Assert.Equal(3, response.Count());
    }
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
    public Task DisposeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StreamerDbContext>();
        DbHelper.ReInitDbForTests(db, DbHelper.GetStreamersForTest());
        return Task.CompletedTask;
    }
}
