using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence;
public class StreamerDbContextSeed
{
    public static async Task SeedAsync(StreamerDbContext context, ILogger<StreamerDbContextSeed> logger)
    {
        if (!context.Streamers.Any())
        {
            context.Streamers.AddRange(GetPreconfiguredStreamer());
            await context.SaveChangesAsync();
            logger.LogInformation("[Seed] {context}", typeof(StreamerDbContextSeed).Name);  
        }
    }
    private static IEnumerable<Streamer> GetPreconfiguredStreamer()
    {
        return new List<Streamer>
            {
                new Streamer {CreatedBy = "test1", UpdatedBy = "test1", Name = "Netflex", Url = "http://www.netflex.com" },
                new Streamer {CreatedBy = "test1", UpdatedBy = "test1", Name = "Amazon VIP", Url = "http://www.amazonvip.com" },
            };

    }
}
