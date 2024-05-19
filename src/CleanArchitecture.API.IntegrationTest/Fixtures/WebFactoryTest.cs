using CleanArchitecture.API.IntegrationTest.Utills;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebMotions.Fake.Authentication.JwtBearer;


namespace CleanArchitecture.API.IntegrationTest.Fixtures;

public class WebFactoryTest : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveService<IUserAccessorService>();
            services.AddScoped<IUserAccessorService, UserAccessorServiceForTest>();

            services.RemoveDbContext<StreamerDbContext>();

            services.AddDbContext<StreamerDbContext>(opt =>
            {
                opt.UseInMemoryDatabase($"TestStreamerDbContext");
            });

            services.SeedDataForTest<StreamerDbContext, Streamer>(DbHelper.GetStreamersForTest());
            services.SeedDataForTest<StreamerDbContext, Video>(DbHelper.GetVideosForTests());

            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme)
                .AddFakeJwtBearer(opt =>
                {
                    opt.BearerValueType = FakeJwtBearerBearerValueType.Jwt;
                });
        });
    }
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
