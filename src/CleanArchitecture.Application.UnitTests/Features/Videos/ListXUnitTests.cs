using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Features.Videos.Dtos;
using CleanArchitecture.Application.UnitTests.Mocks;
using CleanArchitecture.Infrastructure.Repositories;

namespace CleanArchitecture.Application.UnitTests.Features.Videos;
public class ListXUnitTests
{
    private readonly IMapper _mapper;
    private readonly IVideoRepository _videoRepository;

    public ListXUnitTests()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfiles>();
        });

        _mapper = mapperConfig.CreateMapper();

        var fakeStreamerDbContext = new FakeStreamerDbContext();
        _videoRepository = Substitute.For<VideoRepository>(fakeStreamerDbContext);
    }

    [Fact]
    public async Task GetVideoListTest()
    {
        // Arrange
        var request = new Application.Features.Videos.List.Query("test");

        // Act
        var handler = new Application.Features.Videos.List.Handler(_videoRepository, _mapper);
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsType<List<VideoDto>>(result);
        Assert.True(result.Count() > 1);
    }
}
