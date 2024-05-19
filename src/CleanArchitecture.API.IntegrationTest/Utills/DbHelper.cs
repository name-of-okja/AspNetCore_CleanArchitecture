using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.API.IntegrationTest.Utills;
public static class DbHelper
{
    public static void InitDbForTest<T, Entity>(T db, ICollection<Entity> entities) where T : DbContext where Entity : BaseDomainModel
    {
        db.Set<Entity>().AddRange(entities);
        db.SaveChanges();
    }

    public static void ReInitDbForTests<T, Entity>(T db, ICollection<Entity> entities) where T : DbContext where Entity : BaseDomainModel
    {
        var dbEntities = db.Set<Entity>().ToList();
        db.Set<Entity>().RemoveRange(dbEntities);
        db.SaveChanges();
        InitDbForTest(db, entities);
    }

    public static List<Streamer> GetStreamersForTest()
    {
        return new List<Streamer>
        {
             new Streamer
             {
                 Id = 1,
                 Name = "testStreamer1",
                 Url = "www.test.com"
             },
             new Streamer
             {
                 Id = 2,
                 Name = "testStreamer2",
                 Url = "www.test2.com"
             },
             new Streamer
             {
                 Id = 3,
                 Name = "testStreamer3",
                 Url = "www.test3.com"
             }
        };
    }

    public static List<Video> GetVideosForTests()
    {
        return new List<Video>
        {
            new Video
            {
                Id= 1,
                Name = "testVideo1",
                StreamerId = 1,
            },
            new Video
            {
                Id=2,
                Name ="testVideo2",
                StreamerId=2,
            }
        };
    }
}
