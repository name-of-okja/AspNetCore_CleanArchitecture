using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.UnitTests.Mocks;
public  class FakeStreamerDbContext : StreamerDbContext
{

    public FakeStreamerDbContext() 
        : base(new DbContextOptionsBuilder<StreamerDbContext>()
                    .UseInMemoryDatabase(databaseName: $"StreamerDbContext-{Guid.NewGuid()}")
                    .EnableSensitiveDataLogging()
                    .Options
            , FakeUserAccessorService.GetUserAccessorService())
    {
        Seed(this);
    }

    private static void Seed(StreamerDbContext context)
    {
        if (!context.Streamers.Any())
        {
            context.Streamers.AddRange(GetPreconfiguredStreamer());
            context.SaveChanges();
        }

        if (!context.Videos.Any())
        {
            var netflex = context.Streamers.First(x => x.Name == "Netflex");
            var amazon = context.Streamers.First(x => x.Name == "Amazon VIP");

            context.Videos.AddRange(GetPreconfiguredVideo(netflex.Id, amazon.Id));
            context.SaveChanges();
        }
    }
    private static IEnumerable<Streamer> GetPreconfiguredStreamer()
    {
        return new List<Streamer>
            {
                new Streamer { Name = "Netflex", Url = "http://www.netflex.com" },
                new Streamer { Name = "Amazon VIP", Url = "http://www.amazonvip.com" },
                new Streamer { Name = "TestStreamer", Url ="www.test.com"}
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
