using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure.Persistence;
namespace CleanArchitecture.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructureServcies(builder.Configuration);
        builder.Services.AddApplicationServcies();

        var app = builder.Build();

        using (var dbSeedScope = app.Services.CreateScope())
        {
            await StreamerDbContextSeed.SeedAsync(dbSeedScope.ServiceProvider.GetRequiredService<StreamerDbContext>(),
                                    dbSeedScope.ServiceProvider.GetRequiredService<ILogger<StreamerDbContextSeed>>());
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
