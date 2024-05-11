using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models.Email;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure;
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServcies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StreamerDbContext>(opt => 
            opt.UseSqlite(configuration.GetConnectionString("StreamerConnectionString"))
        );

        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddLazyScoped<IVideoRepository, VideoRepository>();
        services.AddLazyScoped<IStreamerRepository, StreamerRepository>();
        services.AddLazyScoped<IDirectorRepository, DirectorRepository>();


        services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();
  
        return services;
    }

    private static void AddLazyScoped<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TInterface, TImplementation>();
        services.AddScoped<Lazy<TInterface>>(provider =>
            new Lazy<TInterface>(() => provider.GetRequiredService<TInterface>()));
    }
}
