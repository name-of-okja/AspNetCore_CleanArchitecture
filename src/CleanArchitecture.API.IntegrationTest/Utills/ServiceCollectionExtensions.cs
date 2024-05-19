using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.API.IntegrationTest.Utills;
public  static class ServiceCollectionExtensions
{
    public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }

    public static void SeedDataForTest<T, Entity>(this IServiceCollection services, ICollection<Entity> entities) where T : DbContext where Entity : BaseDomainModel
    {
        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<T>();
        
        DbHelper.InitDbForTest(db, entities);
    }

    public static void RemoveService<T>(this IServiceCollection services) where T : class
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }
}
