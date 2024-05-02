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

        if (!context.Videos.Any())
        {
            var netflex = context.Streamers.First(x => x.Name == "Netflex");
            var amazon = context.Streamers.First(x => x.Name == "Amazon VIP");

            context.Videos.AddRange(GetPreconfiguredVideo(netflex.Id, amazon.Id));
            await context.SaveChangesAsync();
            logger.LogInformation("[Seed] {context}", typeof(StreamerDbContextSeed).Name);
        }
    }
    private static IEnumerable<Streamer> GetPreconfiguredStreamer()
    {
        return new List<Streamer>
            {
                new Streamer { Name = "Netflex", Url = "http://www.netflex.com" },
                new Streamer { Name = "Amazon VIP", Url = "http://www.amazonvip.com" },
            };

    }

    private static IEnumerable<Video> GetPreconfiguredVideo(int netflexId, int amazonId)
    {
        return new List<Video>
            {
                new Video { Name = "범죄도시1", StreamerId = netflexId },
                new Video { Name = "범죄도시2", StreamerId = netflexId},
                new Video { Name = "해리포터1", StreamerId = amazonId},
                new Video { Name = "친구", StreamerId = amazonId },
            };
    }
}
