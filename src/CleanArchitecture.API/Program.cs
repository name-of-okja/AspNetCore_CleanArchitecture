using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Identity;
using CleanArchitecture.API.Middleware;
namespace CleanArchitecture.API;

public class Program
{
    internal static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructureServcies(builder.Configuration);
        builder.Services.AddApplicationServcies();
        builder.Services.AddAuthServices(builder.Configuration);
        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", 
                builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
        });

        var app = builder.Build();

        using (var dbSeedScope = app.Services.CreateScope())
        {
            await StreamerDbContextSeed.SeedAsync(
                                        dbSeedScope.ServiceProvider.GetRequiredService<StreamerDbContext>(),
                                        dbSeedScope.ServiceProvider.GetRequiredService<ILogger<StreamerDbContextSeed>>()
                                    );
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("CorsPolicy");

        app.MapControllers();

        app.Run();
    }
}
